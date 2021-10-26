using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [AllureNUnit]
    [TestFixture]
    public class POST_AddNewStudent : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public POST_AddNewStudent()
        {
            api.log = LogManager.GetLogger($"Students/{nameof(POST_AddNewStudent)}");
        }

        /// <summary>
        /// That auto test verify adding new students using POST request / Student section with Admin/Secretary role
        ///
        /// Steps:
        /// 1.Generate student and register him using POST request gor registration users / Account section
        /// 2.Find student ID at the end of active students list using GET request / Account section
        /// 3.Execute adding new students using POST request / Students section
        /// 4.Find new student in students list using GET request / Students section
        /// 
        /// Postcondition:
        /// 1. Delete this student from list using DELETE request / Student section
        /// </summary>
        [Test]
        [TestCase (Role.Admin)]
        [TestCase (Role.Secretary)]
        public void VerifyAddingStudentAccount_Valid(Role role)
        {
            var expectedUser = new GenerateUser();
            string expectedUserAvatarUrl = null;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(expectedUser);
            response = api.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");

            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", api.endpointsPath)}");
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(role));
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");

            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id); ;
            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath)}");
            request = api.InitNewRequest("ApiStudentsAccountId", Method.POST, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = api.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");

            api.log.Info($"POST request to {response.ResponseUri}");
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(role));
            response = APIClient.client.Execute(request);

            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiStudentsActive", api.endpointsPath)}");
            var listActiveStudents = JsonConvert.DeserializeObject<List<StudentDetails>>(response.Content);
            int maxId = listActiveStudents.Max(i=>i.Id);
            var actualUser = listActiveStudents.First(x => x.Id == maxId);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
                Assert.AreEqual(expectedUser.Email, actualUser.Email);
                Assert.AreEqual(expectedUserAvatarUrl, actualUser.AvatarUrl);
                api.log.Info($"Expected and actual results is checked");
            });
            PostCondition(role, maxId);
            api.log.Info($"Last student in list is deleted");
        }

        private void PostCondition(Role role, int lastUserId)
        {
            request = api.InitNewRequest("ApiStudentsId", Method.DELETE, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("id", lastUserId.ToString());
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
        }
    }
}
