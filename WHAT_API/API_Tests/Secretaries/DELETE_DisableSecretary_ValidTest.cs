using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;
using NUnit.Allure.Core;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class DELETE_DisableSecretary_ValidTest: API_BaseTest
    {
        Random random = new Random();
        private Account registeredUser;
        int SecretaryID { get; set; }

        public DELETE_DisableSecretary_ValidTest()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(DELETE_DisableSecretary_ValidTest)}");
        }

        [OneTimeSetUp]
        public void PreConditions()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            registeredUser = RegistrationUser();
            RestRequest request = InitNewRequest("ApiAccountsNotAssigned", Method.GET, authenticator);
            IRestResponse response = client.Execute(request);

            string json = response.Content;
            var users = JsonConvert.DeserializeObject<List<Account>>(json);
            var searchedUser = users.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            registeredUser.Id = searchedUser.Id;

            request = InitNewRequest("ApiSecretariesAccountId", Method.POST, authenticator);
            request.AddUrlSegment("accountId", registeredUser.Id.ToString());
            response = client.Execute(request);

            request = new RestRequest(ReaderUrlsJSON.ByName("GET All Secretaries", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            response = client.Execute(request);

            List<Secretary> secretaries = JsonConvert.DeserializeObject<List<Secretary>>(response.Content.ToString());
            var searchedSecretary = secretaries.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            SecretaryID = searchedSecretary.Id;
        }

        [Test, TestCase(HttpStatusCode.OK, "true")]
        public void DELETE_DisableTest(HttpStatusCode expectedStatus, string expectedResponse)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.DELETE);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            var actualStatus = response.StatusCode;
            string responseActual = response.Content;

            Assert.AreEqual(expectedStatus, actualStatus);

            Assert.AreEqual(expectedResponse, responseActual);
        }
    }
}
