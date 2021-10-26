using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [AllureNUnit]
    [TestFixture]
    public class PATCH_EnableStudetnsAccount:API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        public PATCH_EnableStudetnsAccount()
        {
            api.log = LogManager.GetLogger($"Students/{nameof(PATCH_EnableStudetnsAccount)}");
        }

        private void Precondition(Role role)
        {
            var expectedUser = new GenerateUser();
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(expectedUser);
            response = APIClient.client.Execute(request);

            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", api.endpointsPath)}");
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(role));
            response = APIClient.client.Execute(request);

            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath)}");
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsAccountId", api.endpointsPath), Method.POST);
            request = api.InitNewRequest("ApiStudentsAccountId", Method.POST, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = APIClient.client.Execute(request);

            int lastUserId = GetActiveStudentsList("ApiStudentsActive", role).Last().Id;
            request = api.InitNewRequest("ApiStudentsId", Method.DELETE, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("id", lastUserId.ToString());
            response = APIClient.client.Execute(request);
            api.log.Info($"Last student in list is deleted");
        }

        /// <summary>
        /// Verify deleting students using DELETE request / Student section with Admin/Secretary role
        /// </summary>
        /// <remarks>
        /// Precondition:
        /// 1.Generate student and register him using POST request gor registration users / Account section
        /// 2.Find student ID at the end of active students list using GET request / Account section
        /// 3.Execute adding new students using POST request / Students section
        /// 5.Get last user id from active students list using GET request / Account section
        /// 6.Delete this student from list using DELETE request / Student section
        /// 
        /// Steps:
        /// 1. Get list of all students from student list using GET request/ Student section
        /// 2. Get list of active students from student list using GET request/ Student section
        /// 3. Take away list of active students from the list of all students and get first student
        /// 4. Enable student account using PATCH request/ Student section
        /// 5. Find student ID at the end of active students list using GET request / Account section
        ///</remarks>
        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyEnablingStudentAccount_Valid(Role role)
        {
            Precondition(role);
            var allStudents = GetActiveStudentsList("ApiStudents", role).ToList();
            var activeStudents = GetActiveStudentsList("ApiStudentsActive", role).ToList();
            var expectedStudent = allStudents.Except(activeStudents).ToList().OrderBy(x=>x.Id).First();
            request = api.InitNewRequest("ApiStudentsId", Method.PATCH, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("id", expectedStudent.Id.ToString());
            api.log.Info($"PATCH request to {response.ResponseUri}");
            response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
            var actualStudent = GetActiveStudentsList("ApiStudentsActive", role).Where(x=>x.Id==expectedStudent.Id).First();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedStudent.FirstName, actualStudent.FirstName);
                Assert.AreEqual(expectedStudent.LastName, actualStudent.LastName);
                Assert.AreEqual(expectedStudent.Email, actualStudent.Email);
                Assert.AreEqual(expectedStudent.AvatarUrl, actualStudent.AvatarUrl);
                api.log.Info($"Expected and actual results is checked");
            });
        }

        private List<StudentDetails> GetActiveStudentsList(string endpoint, Role role)
        {
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", api.GetToken(role));
            IRestResponse getResponse = APIClient.client.Execute(getRequest);
            api.log.Info($"GET request to {response.ResponseUri}");
            return JsonConvert.DeserializeObject<List<StudentDetails>>(getResponse.Content);
        }
    }
}
