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
    public class DELETE_DisableStudentAccount:API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public DELETE_DisableStudentAccount()
        {
            api.log = LogManager.GetLogger($"Students/{nameof(DELETE_DisableStudentAccount)}");
        }

        private void Precondition(Role role)
        {
            var expectedUser = new GenerateUser();
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(expectedUser);
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");

            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath)}");
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(role));
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            api.log.Info($"Get user id: {newUserAccountId}");

            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiStudentsAccountId", api.endpointsPath)}");
            request = api.InitNewRequest("ApiStudentsAccountId", Method.POST, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
        }

        /// <summary>
        /// Verify deleting students using DELETE request / Student section with Admin/Secretary role
        /// </summary>
        /// <remarks>
        /// Precondition:
        /// 1.Generate student and register him using POST request gor registration users / Account section
        /// 2.Find student ID at the end of active students list using GET request / Account section
        /// 3.Execute adding new students using POST request / Students section
        ///
        /// Steps:
        /// 1.Get list of active students list using GET request / Account section
        /// 2.Delete this student from list using DELETE request / Student section
        /// 3.Сheck that the list has decreased by one
        ///</remarks>
        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyDeletingStudentAccount_Valid(Role role)
        {
            Precondition(role);
            int lastUserId = GetActiveStudentsList(role).Last().Id;
            int expect = GetActiveStudentsList(role).Count-1;
            api.log.Info($"List of students is taken, there are {GetActiveStudentsList(role).Count} active students");

            api.log.Info($"DELETE request to {ReaderUrlsJSON.ByName("ApiStudentsId", api.endpointsPath)}");
            request = api.InitNewRequest("ApiStudentsId", Method.DELETE, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("id", lastUserId.ToString());
            response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            api.log.Info($"Deleted students with max id : {lastUserId}");
            int actual = GetActiveStudentsList(role).Count;
            api.log.Info($"List of students is taken, there are {GetActiveStudentsList(role).Count} active students");
            Assert.AreEqual(expect, actual);
        }

        /// <summary> Get list of active students using GET request / Student section</summary>
        /// <param name="role"> User role </param>
        /// <returns> StudentResponseBody entity </returns>
        private List<StudentDetails> GetActiveStudentsList(Role role)
        {
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", api.endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", api.GetToken(role));
            IRestResponse getResponse = APIClient.client.Execute(getRequest);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
            return JsonConvert.DeserializeObject<List<StudentDetails>>(getResponse.Content);
        }
    }
}
