using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [TestFixture]
    public class POST_AddNewStudent : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        [Test]
        [TestCase (Role.Admin)]
        [TestCase(Role.Secretar)]
        public void VerifyAddingStudentAccount_Valid(Role role)
        {
            var expectedUser = UserGenerator.GenerateUser();
            string expectedUserAvatarUrl = null;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(expectedUser);
            response = client.Execute(request);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            //
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);
            int newUserAccountId= JsonConvert.DeserializeObject<List<RegistrationResponseBody>>(response.Content).Max(s => s.Id); ;
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");
            //
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsAccountId", endpointsPath), Method.POST);
            request = InitNewRequest("ApiStudentsAccountId", Method.POST, GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = client.Execute(request);
            log.Info($"POST request to {response.ResponseUri}");
            //
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiStudentsActive", endpointsPath)}");
            var listActiveStudents = JsonConvert.DeserializeObject<List<StudentResponseBody>>(response.Content); ;
            int maxId = listActiveStudents.Max(i=>i.Id);
            var actualUser = listActiveStudents.First(x => x.Id == maxId);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
                Assert.AreEqual(expectedUser.Email, actualUser.Email);
                Assert.AreEqual(expectedUserAvatarUrl, actualUser.AvatarUrl);
            });
            log.Info($"Expected and actual results is checked");

        }
    }
}
