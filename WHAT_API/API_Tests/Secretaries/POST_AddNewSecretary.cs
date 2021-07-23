using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    class POST_AddNewSecretary : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public POST_AddNewSecretary()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(POST_AddNewSecretary)}");
        }

        private IRestResponse POST_ApiAccountsReg(GenerateUser user)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(user);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            return Execute(request);
        }

        private IRestResponse GET_ApiAccountsNotAssigned()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");
            return Execute(request);
        }

        private IRestResponse POST_ApiSecretariesAccountId(Role role, int accountId)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesAccountId", endpointsPath), Method.POST);
            request = InitNewRequest("ApiSecretariesAccountId", Method.POST, GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", accountId.ToString());
            request.AddParameter("accountId", accountId);
            response = Execute(request);
            log.Info($"POST request to {response.ResponseUri}");
            return response;
        }

        private IRestResponse GET_ApiSecretariesActive()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath)}");
            return Execute(request);
        }

        private IRestResponse DELETE_ApiSecretariesId(int userId)
        {
            request = InitNewRequest("ApiSecretariesId", Method.DELETE, GetAuthenticatorFor(Role.Admin));
            request.AddUrlSegment("id", userId.ToString());
            log.Info($"Last secretary in list is deleted");
            return Execute(request);
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_Valid(Role role)
        {
            var expectedUser = new GenerateUser();
            POST_ApiAccountsReg(expectedUser);
            response = GET_ApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            POST_ApiSecretariesAccountId(role, newUserAccountId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response = GET_ApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
                Assert.AreEqual(expectedUser.Email, actualUser.Email);
            });
            log.Info($"Expected and actual results is checked");
            DELETE_ApiSecretariesId(maxId);
        }

        [Test]
        [TestCase(Role.Student)]
        [TestCase(Role.Mentor)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Unassigned)]
        public void VerifyAddingSecretaryAccount_Forbidden(Role role)
        {
            var expectedUser = new GenerateUser();
            POST_ApiAccountsReg(expectedUser);
            response = GET_ApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            response = POST_ApiSecretariesAccountId(role, newUserAccountId);
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            response = GET_ApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            Assert.AreNotEqual(expectedUser.Email, actualUser.Email);
            log.Info($"Expected and actual results is checked");
            DELETE_ApiSecretariesId(maxId);
        }

        [Test]
        public void VerifyAddingSecretaryAccount_Unauthorized()
        {
            var expectedUser = new GenerateUser();
            POST_ApiAccountsReg(expectedUser);
            response = GET_ApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesAccountId", endpointsPath), Method.POST);
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = Execute(request);
            log.Info($"POST request to {response.ResponseUri}");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            response = GET_ApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            Assert.AreNotEqual(expectedUser.Email, actualUser.Email);
            log.Info($"Expected and actual results is checked");
            DELETE_ApiSecretariesId(maxId);
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_AccountNotFound(Role role)
        {
            response = GET_ApiAccountsNotAssigned();
            int maxUserId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            response = POST_ApiSecretariesAccountId(role, maxUserId + 1);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            response = GET_ApiSecretariesActive();
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_AlreadyAssigned(Role role)
        {
            var expected = HttpStatusCode.BadRequest;
            var newUser = new GenerateUser();
            POST_ApiAccountsReg(newUser);
            response = GET_ApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            POST_ApiSecretariesAccountId(role, newUserAccountId);
            POST_ApiSecretariesAccountId(role, newUserAccountId);
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
            response = GET_ApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            log.Info($"Expected and actual results is checked");
            DELETE_ApiSecretariesId(maxId);
        }
    }
}

        