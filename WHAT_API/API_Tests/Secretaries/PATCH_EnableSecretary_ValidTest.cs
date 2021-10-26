using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;
using NLog;
using NUnit.Allure.Core;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class PATCH_EnableSecretary_ValidTest: API_BaseTest
    {
        Random random = new Random();
        private WhatAccount registeredUser;
        int SecretaryID { get; set; }

        public PATCH_EnableSecretary_ValidTest()
        {
            api.log = LogManager.GetLogger($"Secretaries/{nameof(PATCH_EnableSecretary_ValidTest)}");
        }

        [OneTimeSetUp]
        public void PreConditions()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            registeredUser = api.RegistrationUser();
            RestRequest request = api.InitNewRequest("ApiAccountsNotAssigned", Method.GET, authenticator);
            IRestResponse response = APIClient.client.Execute(request);

            string json = response.Content;
            var users = JsonConvert.DeserializeObject<List<Account>>(json);
            var searchedUser = users.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            registeredUser.Id = searchedUser.Id;

            request = api.InitNewRequest("ApiSecretariesAccountId", Method.POST, authenticator);
            request.AddUrlSegment("accountId", registeredUser.Id.ToString());
            response = APIClient.client.Execute(request);

            request = new RestRequest(ReaderUrlsJSON.ByName("GET All Secretaries", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));
            response = APIClient.client.Execute(request);

            List<Secretary> secretaries = JsonConvert.DeserializeObject<List<Secretary>>(response.Content.ToString());
            var searchedSecretary = secretaries.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            SecretaryID = searchedSecretary.Id;

            RestRequest deleteRequest = new RestRequest($"secretaries/{SecretaryID}", Method.DELETE);
            deleteRequest.AddHeader("Authorization", api.GetToken(Role.Admin));

            IRestResponse deleteResponse = APIClient.client.Execute(deleteRequest);

            if (deleteResponse.StatusCode != HttpStatusCode.OK && deleteResponse.Content.ToString() != "true")
            {
                throw new Exception($"Status code: {deleteResponse.StatusCode} is not {HttpStatusCode.OK}");
            }
        }

        [Test, TestCase(HttpStatusCode.OK)]
        public void PATCH_EnableTest(HttpStatusCode expectedStatus)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PATCH);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));

            IRestResponse response = APIClient.client.Execute(request);

            var actualStatus = response.StatusCode;

            Assert.AreEqual(expectedStatus, actualStatus);
        }
    }
}
