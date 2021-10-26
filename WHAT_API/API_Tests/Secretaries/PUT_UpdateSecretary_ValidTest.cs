using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using NLog;
using System.Linq;
using System.Net;
using WHAT_Utilities;
using NUnit.Allure.Core;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class PUT_UpdateSecretary_ValidTest : API_BaseTest
    {
        Random random = new Random();
        private WhatAccount registeredUser;

        int SecretaryID { get; set; }

        public PUT_UpdateSecretary_ValidTest()
        {
            api.log = LogManager.GetLogger($"Secretaries/{nameof(PUT_UpdateSecretary_ValidTest)}");
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
        }


        [Test,TestCase(HttpStatusCode.OK, "@gmail.com", "Afal", "Barl")]
        public void PUT_UpdateTest(HttpStatusCode expectedStatus, string email, string firstName, string lastName)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PUT);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));
            email = $"{Guid.NewGuid():N}" + email;
            Secretary body = new Secretary()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            request.AddJsonBody(body);
            IRestResponse response = APIClient.client.Execute(request);

            var actualStatus = response.StatusCode;
            var responseSecretary = JsonConvert.DeserializeObject<Secretary>(response.Content.ToString());

            Assert.AreEqual(expectedStatus, actualStatus);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(email, responseSecretary.Email);
                Assert.AreEqual(firstName, responseSecretary.FirstName);
                Assert.AreEqual(lastName, responseSecretary.LastName);            
            });
        }
    }
}
