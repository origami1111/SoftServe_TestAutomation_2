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
            api.log = LogManager.GetLogger($"Secretaries/{nameof(GET_GetAllSecretaries)}");
        }

        private IRestResponse GetApiAccountsAll()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAll", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));
            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsAll", api.endpointsPath)}");
            return api.Execute(request);
        }

        private IRestResponse GetApiSecretaries(Role role)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretaries", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(role));
            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretaries", api.endpointsPath)}");
            return api.Execute(request);
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
            api.log.Info($"Expected and actual results is checked");
        }

        [Test]
        public void VerifyGettingAllSecretaries_Unauthorized()
        {
            var expected = HttpStatusCode.Unauthorized;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretaries", api.endpointsPath), Method.GET);
            response = api.Execute(request);
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
            api.log.Info($"Expected and actual results is checked");
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
            api.log.Info($"Expected and actual results is checked");
        }

    }
}

