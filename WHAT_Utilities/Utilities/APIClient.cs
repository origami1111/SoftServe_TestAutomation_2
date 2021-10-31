using Newtonsoft.Json;
using NLog;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WHAT_Utilities
{
    public class APIClient
    {
        public string endpointsPath = @"DataFiles/Endpoints.json";
        public static string linksPath = @"DataFiles/Links.json";
        public static RestClient client =
            new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
        public Logger log = LogManager.GetCurrentClassLogger();

        private readonly IAuthenticator[] authenticators =
            new IAuthenticator[Enum.GetValues(typeof(Role)).Length];

        public RestRequest InitNewRequest(string endPointName, Method method,
            IAuthenticator authenticator)
        {
            var resource = ReaderUrlsJSON.ByName(endPointName, endpointsPath);
            var request = new RestRequest(resource, method);
            authenticator.Authenticate(client, request);

            return request;
        }

        public IRestResponse Execute(RestRequest request)
        {
            return Execute<object>(request);
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var response = client.Execute<T>(request);

            var text = new StringBuilder();
            text.Append(request.Method).Append(" ")
                .Append(client.BuildUri(request).AbsolutePath)
                .Append("  Http Status Code: ").AppendLine(response.StatusDescription);

            if (request.Body != null)
            {
                text.Append("Request body: ").AppendLine(request.Body.Value?.ToString());
            }

            if (response.ErrorException != null)
            {
                text.AppendLine(response.ErrorException.Message);
                log.Error(text.ToString());

                const string message = "Error retrieving response. Check inner details for more info.";
                throw new Exception(message, response.ErrorException);
            }

            if (response.Content != null)
            {
                text.Append("Response content: ").Append(response.Content);
            }

            log.Info(text.ToString());
            return response;
        }

        public string GetToken(Role role)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
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

        public string GetToken(Role role, RestClient client)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            credentials.Role = role;
            return GetToken(credentials);
        }

        public string GetToken(Credentials credentials)
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

        public IAuthenticator GetAuthenticatorFor(Role role)
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

        public IAuthenticator GetAuthenticatorFor(Credentials credentials)
        {
            string accessToken = GetToken(credentials);
            if (accessToken.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                accessToken = accessToken["Bearer ".Length..];                
            }
            IAuthenticator authenticator = new JwtAuthenticator(accessToken);
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

        public WhatAccount RegistrationUser()
        {
            var userInfo = new GenerateUser();

            return RegistrationUser(userInfo);
        }

        public WhatAccount RegistrationUser(GenerateUser userInfo)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(userInfo);
            client.Execute(request);

            var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
            var getUnassignedUsersRequest = InitNewRequest("ApiAccountsNotAssigned", Method.GET, adminAuthenticator);

            var unassignedUser = Execute<List<WhatAccount>>(getUnassignedUsersRequest).Data.First(u => u.Email == userInfo.Email);
            unassignedUser.Activity = Activity.Active;

            return unassignedUser;
        }

        public WhatAccount AssignRole(WhatAccount user, Role role)
        {
            if (role != Role.Unassigned)
            {
                string endpoint = role switch
                {
                    Role.Mentor => "ApiMentorId",
                    Role.Secretary => "ApiSecretariesAccountId",
                    Role.Student => "ApiStudentsAccountId",
                    _ => throw new NotSupportedException()
                };
                var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
                var assignRoleRequest = InitNewRequest(endpoint, Method.POST, adminAuthenticator);
                assignRoleRequest.AddUrlSegment("accountId", user.Id.ToString());
                IRestResponse assignRoleResponse = client.Execute(assignRoleRequest);
                string assignJson = assignRoleResponse.Content;
                WhatAccount account = JsonConvert.DeserializeObject<WhatAccount>(assignJson);
                return account;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void DisableAccount(WhatAccount user, Role role)
        {
            if (role != Role.Unassigned)
            {
                string endpoint = role switch
                {
                    Role.Mentor => "ApiMentorId",
                    Role.Secretary => "ApiSecretariesAccountId",
                    Role.Student => "ApiStudentsAccountId",
                    _ => throw new NotSupportedException()
                };
                var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
                var disableRequest = InitNewRequest(endpoint, Method.DELETE, adminAuthenticator);
                disableRequest.AddUrlSegment("accountId", user.Id.ToString());
                IRestResponse disableResponse = client.Execute(disableRequest);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public CourseDto CreateCourse(CreateCourseDto course)
        {
            var endpoint = "Add new course";
            var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
            var request = InitNewRequest(endpoint, Method.POST, adminAuthenticator);
            request.AddJsonBody(course);
            IRestResponse response = client.Execute(request);
            string responseJson = response.Content;
            CourseDto CourseResponse = JsonConvert.DeserializeObject<CourseDto>(responseJson);
            return CourseResponse;
        }

        public void DisableCourse(CourseDto course)
        {
            var endpoint = "Disable course";
            var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
            var request = InitNewRequest(endpoint, Method.DELETE, adminAuthenticator);
            request.AddUrlSegment("id", course.Id.ToString());
            IRestResponse response = client.Execute(request);
        }

        public StudentGroupDto CreateStudentGroup(CreateStudentGroupDto studentGroup)
        {
            var endpoint = "ApiStudentsGroup";
            var adminAuthenticator = GetAuthenticatorFor(Role.Admin);
            var request = InitNewRequest(endpoint, Method.POST, adminAuthenticator);
            request.AddJsonBody(studentGroup);
            IRestResponse response = client.Execute(request);
            string responseJson = response.Content;
            StudentGroupDto StudentGroupResponse = JsonConvert.DeserializeObject<StudentGroupDto>(responseJson);
            return StudentGroupResponse;
        }
    }
}
