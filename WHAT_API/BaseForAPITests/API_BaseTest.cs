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
        protected internal const string endpointsPath = @"DataFiles/Endpoints.json";
        protected internal const string linksPath = @"DataFiles/Links.json";
        
        protected internal static RestClient client =
            new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", API_BaseTest.linksPath));
        protected internal Logger log = LogManager.GetCurrentClassLogger();
        
        private readonly IAuthenticator[] authenticators =
            new IAuthenticator[Enum.GetValues(typeof(Role)).Length];
        
        [TearDown]
        public void LogAfterEachTest()
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
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { credentials.Email, credentials.Password });
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                log.Info($"Successfully get token for {role}");
                return response.Headers.Single(h => h.Name == "Authorization").Value.ToString();
            }
            else
            {
                var message = $"Authorization is failed for {credentials.Role}: {credentials.Email}.";
                log.Error(message);
                throw new Exception(message);
            }
        }

        protected string GetToken(Role role, RestClient client)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

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
                log.Info($"Successfully get token by role {credentials.Role}");
                return response.Headers.Single(h => h.Name == "Authorization").Value.ToString();
            }
            else
            {
                var message = $"Authorization is failed for {credentials.Role}: {credentials.Email}.";
                log.Error(message);
                throw new Exception(message);
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
            try
            {
                accessToken = GetToken(role);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                if (role != Role.Admin)
                {
                    Credentials credentials = GetUserWithRole(role);
                    accessToken = GetToken(credentials);
                }
            }

            if (accessToken.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                accessToken = accessToken["Bearer ".Length..];
            }

            authenticator = new JwtAuthenticator(accessToken);
            authenticators[(int)role] = authenticator;
            return authenticator;
        }

        private Credentials GetUserWithRole(Role role)
        {
            var userInfo = new GenerateUser();
            var newUser = RegistrationUser(userInfo);
            newUser.LastName = $"{role}_{newUser.Email.Substring(0, 5)}";
            AssignRole(newUser, role);
            var credentials = new Credentials
            {
                Email = userInfo.Email,
                Password = userInfo.Password,
                Role = role
            };
            return credentials;
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

        protected Account RegistrationUser()
        {
            var userInfo = new GenerateUser();

            return RegistrationUser(userInfo);
        }

        protected Account RegistrationUser(GenerateUser userInfo)
        {
            RestRequest request =
                new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(userInfo);
            client.Execute(request);

            var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
            var getUnassignedUsersRequest =
                InitNewRequest("ApiAccountsNotAssigned", Method.GET, adminAuthenticator);

            var unassignedUser = Execute<List<Account>>(getUnassignedUsersRequest)
                .First(u => u.Email == userInfo.Email);
            unassignedUser.Activity = Activity.Active;

            return unassignedUser;
        }

        protected void AssignRole(Account user, Role role)
        {
            if (role != Role.Unassigned)
            {
                string endpoint = role switch
                {
                    Role.Mentor => "ApiMentorsAssignAccountToMentor-accountID",
                    Role.Secretary => "ApiSecretariesAccountId",
                    Role.Student => "ApiStudentsAccountId",
                    _ => throw new NotSupportedException()
                };
                var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
                var assignRoleRequest = InitNewRequest(endpoint, Method.POST, adminAuthenticator);
                assignRoleRequest.AddUrlSegment("accountId", user.Id.ToString());
                client.Execute(assignRoleRequest);
            }
        }
    }
}