using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using NLog;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class PUT_UpdateSecretary_ValidTest : API_BaseTest
    {
        Random random = new Random();
        private Account registeredUser;

        int SecretaryID { get; set; }

        public PUT_UpdateSecretary_ValidTest()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(PUT_UpdateSecretary_ValidTest)}");
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
            SecretaryID = searchedSecretary.ID;
        }


        [Test,TestCase(HttpStatusCode.OK, "@gmail.com", "Afal", "Barl")]
        public void PUT_UpdateTest(HttpStatusCode expectedStatus, string email, string firstName, string lastName)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PUT);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            email = $"{Guid.NewGuid():N}" + email;
            Secretary body = new Secretary()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            request.AddJsonBody(body);
            IRestResponse response = client.Execute(request);

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
