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
    class GET_GetActiveSecretaries : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public GET_GetActiveSecretaries()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(GET_GetActiveSecretaries)}");
        }
        private IRestResponse GetApiAccountsAll()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAll", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsAll", endpointsPath)}");
            return Execute(request);
        }

        private IRestResponse GetApiSecretariesActive(Role role)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath)}");
            return Execute(request);
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyGettingActiveSecretaries_Valid(Role role)
        {
            response = GetApiAccountsAll();
            var expectedSecretariesList = from account in JsonConvert.DeserializeObject<List<Account>>(response.Content)
                                          where account.Role.Equals(Role.Secretary)
                                          where account.Activity.Equals(Activity.Active)
                                          select account;
            response = GetApiSecretariesActive(role);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var actualSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);
            CollectionAssert.AreEquivalent(actualSecretariesList, expectedSecretariesList);
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        public void VerifyGettingAllSecretaries_Unauthorized()
        {
            var expected = HttpStatusCode.Unauthorized;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath), Method.GET);
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
            response = GetApiSecretariesActive(role);
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
            log.Info($"Expected and actual results is checked");
        }
    }
}
