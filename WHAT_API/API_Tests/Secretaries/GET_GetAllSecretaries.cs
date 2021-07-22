using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using WHAT_API.Entities.Secretaries;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    class GET_GetAllSecretaries : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public GET_GetAllSecretaries()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(GET_GetAllSecretaries)}");
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyGettingAllSecretaries_Valid(Role role)
        {
            //GET Accounts
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAll", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            response = client.Execute(request);
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsAll", endpointsPath)}");
            var expectedSecretariesList = from account in JsonConvert.DeserializeObject<List<Account>>(response.Content)
                                          where account.Role.Equals(Role.Secretary)
                                          select account;

            //GET All Secretaries
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretaries", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath)}");
            var actualSecretariesList = JsonConvert.DeserializeObject<List<Secretary>>(response.Content);

            //Assert.AreEqual(actualSecretariesList.Last().FirstName, expectedSecretariesList.Last().FirstName);
            CollectionAssert.AreEquivalent(actualSecretariesList, expectedSecretariesList);
            
            log.Info($"Expected and actual results is checked");
        }
    }
}

