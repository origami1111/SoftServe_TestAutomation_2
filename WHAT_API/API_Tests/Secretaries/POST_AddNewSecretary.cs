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
    class POST_AddNewSecretary : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public POST_AddNewSecretary()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(POST_AddNewSecretary)}");
        }

        [Test]
        [TestCase(Role.Admin)]
        public void VerifyAddingSecretaryAccount_Valid(Role role)
        {
            //POST
            var expectedUser = new GenerateUser();
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(expectedUser);
            response = client.Execute(request);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");

            //GET
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);
            int newUserAccountId = JsonConvert.DeserializeObject<List<Account>>(response.Content).Max(s => s.Id);
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");

            //POST
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesAccountId", endpointsPath), Method.POST);
            request = InitNewRequest("ApiSecretariesAccountId", Method.POST, GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = client.Execute(request);
            log.Info($"POST request to {response.ResponseUri}");

            //GET
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSecretariesActive", endpointsPath)}");
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

            request = InitNewRequest("ApiSecretariesId", Method.DELETE, GetAuthenticatorFor(Role.Admin));
            request.AddUrlSegment("id", maxId.ToString());
            response = client.Execute(request);
            log.Info($"Last secretary in list is deleted");
        }
    }       
}

        