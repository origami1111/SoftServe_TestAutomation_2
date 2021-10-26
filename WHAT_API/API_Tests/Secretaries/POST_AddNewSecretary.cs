using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    class POST_AddNewSecretary : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public POST_AddNewSecretary()
        {
            api.log = LogManager.GetLogger($"Secretaries/{nameof(POST_AddNewSecretary)}");
        }

        private IRestResponse PostApiAccountsReg(GenerateUser user)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(user);
            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", api.endpointsPath)}");
            return api.Execute(request);
        }

        private IRestResponse GetApiAccountsNotAssigned()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));
            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", api.endpointsPath)}");
            return api.Execute(request);
        }

        private IRestResponse PostApiSecretariesAccountId(Role role, int accountId)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesAccountId", api.endpointsPath), Method.POST);
            request = api.InitNewRequest("ApiSecretariesAccountId", Method.POST, api.GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", accountId.ToString());
            request.AddParameter("accountId", accountId);
            response = api.Execute(request);
            api.log.Info($"POST request to {response.ResponseUri}");
            return response;
        }

        private IRestResponse GetApiSecretariesActive()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesActive", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));
            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretariesActive", api.endpointsPath)}");
            return api.Execute(request);
        }

        private IRestResponse DeleteApiSecretariesId(int userId)
        {
            request = api.InitNewRequest("ApiSecretariesId", Method.DELETE, api.GetAuthenticatorFor(Role.Admin));
            request.AddUrlSegment("id", userId.ToString());
            api.log.Info($"Last secretary in list is deleted");
            return api.Execute(request);
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_Valid(Role role)
        {
            var expectedUser = new GenerateUser();
            PostApiAccountsReg(expectedUser);
            response = GetApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            PostApiSecretariesAccountId(role, newUserAccountId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response = GetApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
                Assert.AreEqual(expectedUser.Email, actualUser.Email);
            });
            api.log.Info($"Expected and actual results is checked");
            DeleteApiSecretariesId(maxId);
        }

        [Test]
        [TestCase(Role.Student)]
        [TestCase(Role.Mentor)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Unassigned)]
        public void VerifyAddingSecretaryAccount_Forbidden(Role role)
        {
            var expectedUser = new GenerateUser();
            PostApiAccountsReg(expectedUser);
            response = GetApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            response = PostApiSecretariesAccountId(role, newUserAccountId);
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            response = GetApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            Assert.AreNotEqual(expectedUser.Email, actualUser.Email);
            api.log.Info($"Expected and actual results is checked");
            DeleteApiSecretariesId(maxId);
        }

        [Test]
        public void VerifyAddingSecretaryAccount_Unauthorized()
        {
            var expectedUser = new GenerateUser();
            PostApiAccountsReg(expectedUser);
            response = GetApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesAccountId", api.endpointsPath), Method.POST);
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = api.Execute(request);
            api.log.Info($"POST request to {response.ResponseUri}");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            response = GetApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            Assert.AreNotEqual(expectedUser.Email, actualUser.Email);
            api.log.Info($"Expected and actual results is checked");
            DeleteApiSecretariesId(maxId);
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_AccountNotFound(Role role)
        {
            response = GetApiAccountsNotAssigned();
            int maxUserId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            response = PostApiSecretariesAccountId(role, maxUserId + 1);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            response = GetApiSecretariesActive();
            api.log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_AlreadyAssigned(Role role)
        {
            var expected = HttpStatusCode.BadRequest;
            var newUser = new GenerateUser();
            PostApiAccountsReg(newUser);
            response = GetApiAccountsNotAssigned();
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            PostApiSecretariesAccountId(role, newUserAccountId);
            PostApiSecretariesAccountId(role, newUserAccountId);
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
            response = GetApiSecretariesActive();
            var activeSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            int maxId = activeSecretariesList.Max(i => i.Id);
            var actualUser = activeSecretariesList.First(x => x.Id == maxId);
            api.log.Info($"Expected and actual results is checked");
            DeleteApiSecretariesId(maxId);
        }
    }
}

        