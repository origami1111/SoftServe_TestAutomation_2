using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    public abstract class API_BaseTest
    {
        [SetUpFixture]
        public class API_TestsSetup
        {
            [OneTimeSetUp]
            public static void OneTimeSetUp()
            {
                var baseUrl = ReaderUrlsJSON.ByName("BaseURLforAPI", API_BaseTest.linksPath);
                API_BaseTest.client = new RestClient(baseUrl);
            }
        }

        protected internal static RestClient client;
        protected internal Logger log = LogManager.GetCurrentClassLogger();
        protected readonly string endpointsPath = @"DataFiles/Endpoints.json";
        protected internal const string linksPath = @"DataFiles/Links.json";
        private readonly IAuthenticator[] authenticators =
            new IAuthenticator[Enum.GetValues(typeof(Role)).Length];

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            var testName = context.Test.FullName;
            if (context.Result.Outcome.Status == TestStatus.Passed)
            {
                log.Info($"{testName} {context.Result.Outcome.Status}");
                return;
            }

            foreach (var assertion in context.Result.Assertions)
            {
                log.Error($"{testName} {assertion.Status}:{Environment.NewLine}{assertion.Message}");
            }
        }

        protected string GetToken(Role role)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                log.Info($"Succesfully get token by role {role}");
                return response.Headers.Single(h => h.Name == "Authorization").Value.ToString();
            }
            else
            {
                log.Error("Authorization is failed!");
                throw new Exception();
            }
        }

        protected string GetToken(Role role, RestClient client)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            credentials.Role = role;
            return GetToken(credentials);
        }

        protected string GetToken(Credentials credentials)
        {
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                log.Info($"Sccesfully get token by role {credentials.Role}");
                return response.Headers.Single(h => h.Name == "Authorization").Value.ToString();
            }
            else
            {
                log.Error("Authorization is failed!");
                throw new Exception();
            }
        }

        protected IAuthenticator GetAuthenticatorFor(Role role)
        {
            IAuthenticator authenticator = authenticators[(int)role];
            if (authenticator != null)
            {
                return authenticator;
            }

            string accessToken = null;

            if (role == Role.Mentor ||
                role == Role.Secretary ||
                role == Role.Student)
            {
                var generatedUser = UserGenerator.GenerateUser();
                generatedUser.LastName = $"{role}_{generatedUser.Email.Substring(0, 5)}";
                var user = RegisterNewUserWithRole(generatedUser, role);
                var credentials = new Credentials
                {
                    Email = generatedUser.Email,
                    Password = generatedUser.Password,
                    Role = role
                };
                accessToken = GetToken(credentials);
            }
            else
            {
                accessToken = GetToken(role);
            }

            if (accessToken.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                accessToken = accessToken["Bearer ".Length..];
            }

            authenticator = new JwtAuthenticator(accessToken);
            authenticators[(int)role] = authenticator;
            return authenticator;
        }

        protected RestRequest InitNewRequest(string endPointName, Method method,
            IAuthenticator authenticator)
        {
            var resource = ReaderUrlsJSON.ByName(endPointName, endpointsPath);
            var request = new RestRequest(resource, method);
            authenticator.Authenticate(client, request);

            return request;
        }

        protected T Execute<T>(RestRequest request) where T : new()
        {
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var exception = new Exception(message, response.ErrorException);
                throw exception;
            }
            System.Diagnostics.Debug.WriteLine(response.Content);

            return response.Data;
        }

        protected RegistrationResponseBody RegistrationUser()
        {
            var user =new GenerateUser();
            return RegistrationUser(user);
        }

        protected RegistrationResponseBody RegistrationUser(RegistrationRequestBody user)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(user);

            client.Execute(request);

            var data = new RegistrationResponseBody()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = Role.Unassigned,
                Activity = Activity.Active
            };

            return data;
        }

        protected RegistrationResponseBody RegisterNewUserWithRole(
            RegistrationRequestBody userInfo, Role role)
        {
            string assignRoleEndPoint = role switch
            {
                Role.Mentor => "ApiMentorsAssignAccountToMentor-accountID",
                Role.Secretary => "ApiSecretariesAccountId",
                Role.Student => "ApiStudentsAccountId",
                _ => throw new NotSupportedException()
            };

            var adminAuthenticator = GetAuthenticatorFor(Role.Admin);

            var newUser = RegistrationUser(userInfo);

            var getUnassignedUsersRequest = InitNewRequest("ApiAccountsNotAssigned", Method.GET, adminAuthenticator);

            var unassignedUsers = Execute<List<RegistrationResponseBody>>(getUnassignedUsersRequest);

            var addedUser = unassignedUsers.FirstOrDefault(user => user.Email == newUser.Email);

            RestRequest assignRoleRequest =
                InitNewRequest(assignRoleEndPoint, Method.POST, adminAuthenticator);
            assignRoleRequest.AddUrlSegment("accountId", addedUser.Id.ToString());

            var assignResponse = client.Execute(assignRoleRequest);

            System.Diagnostics.Debug.WriteLine("Assign role: " + assignResponse.StatusDescription);

           // if (assignResponse.IsSuccessful)
            //    throw new Exception(assignResponse.StatusDescription);

            return addedUser;
        }
    }
}