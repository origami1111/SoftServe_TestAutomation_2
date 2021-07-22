using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;
using NLog;

namespace WHAT_API
{
    [TestFixture]
    public class PATCH_EnableSecretary_ValidTest: API_BaseTest
    {
        Random random = new Random();
        private Account registeredUser;
        int SecretaryID { get; set; }

        public PATCH_EnableSecretary_ValidTest()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(PATCH_EnableSecretary_ValidTest)}");
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

            List<Secretaries> secretaries = JsonConvert.DeserializeObject<List<Secretaries>>(response.Content.ToString());
            var searchedSecretary = secretaries.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            SecretaryID = searchedSecretary.ID;

            RestRequest deleteRequest = new RestRequest($"secretaries/{SecretaryID}", Method.DELETE);
            deleteRequest.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse deleteResponse = client.Execute(deleteRequest);

            if (deleteResponse.StatusCode != HttpStatusCode.OK && deleteResponse.Content.ToString() != "true")
            {
                throw new Exception($"Status code: {deleteResponse.StatusCode} is not {HttpStatusCode.OK}");
            }
        }

        [Test, TestCase(HttpStatusCode.OK)]
        public void PATCH_EnableTest(HttpStatusCode expectedStatus)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PATCH);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            var actualStatus = response.StatusCode;

            Assert.AreEqual(expectedStatus, actualStatus);
        }
    }
}
