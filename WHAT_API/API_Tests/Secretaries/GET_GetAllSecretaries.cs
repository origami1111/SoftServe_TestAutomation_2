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
    class GET_GetAllSecretaries : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public GET_GetAllSecretaries()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(GET_GetAllSecretaries)}");
        }

        private IRestResponse GetApiAccountsAll()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAll", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsAll", endpointsPath)}");
            return Execute(request);
        }

        private IRestResponse GetApiSecretaries(Role role)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretaries", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretaries", endpointsPath)}");
            return Execute(request);
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyGettingAllSecretaries_Valid(Role role)
        {
            response = GetApiAccountsAll();
            var expectedSecretariesList = from account in JsonConvert.DeserializeObject<List<Account>>(response.Content)
                                          where account.Role.Equals(Role.Secretary)
                                          select account;
            response = GetApiSecretaries(role);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var actualSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            CollectionAssert.AreEquivalent(actualSecretariesList, expectedSecretariesList);
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        public void VerifyGettingAllSecretaries_Unauthorized()
        {
            var expected = HttpStatusCode.Unauthorized;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretaries", endpointsPath), Method.GET);
            response = Execute(request);
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
            log.Info($"Expected and actual results is checked");
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Unassigned)]
        [Test]
        public void VerifyGettingAllSecretaries_Forbidden(Role role)
        {
            var expected = HttpStatusCode.Forbidden;
            response = GetApiSecretaries(role);
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
            log.Info($"Expected and actual results is checked");
        }

    }
}

