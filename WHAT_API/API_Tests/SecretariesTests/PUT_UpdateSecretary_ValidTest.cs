using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class PUT_UpdateSecretary_ValidTest : API_BaseTest
    {
        Random random = new Random();

        int SecretaryID { get; set; }
        string SecretaryEmail { get; set; }
        string SecretaryFirstName { get; set; }
        string SecretaryLastName { get; set; }

        [OneTimeSetUp]
        public void PreConditions()
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("GET All Secretaries", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            List<Secretaries> secretaries = JsonConvert.DeserializeObject<List<Secretaries>>(response.Content.ToString());
            if (!secretaries.Any() || response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
            else
            {
                int randomElement = random.Next(0, secretaries.Count);
                SecretaryID = secretaries.ElementAt(randomElement).ID;
                SecretaryEmail = secretaries.ElementAt(randomElement).Email;
                SecretaryFirstName = secretaries.ElementAt(randomElement).FirstName;
                SecretaryLastName = secretaries.ElementAt(randomElement).LastName;
            }
        }

        [OneTimeTearDown]
        public void PostConditions()
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PUT);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            Secretaries body = new Secretaries()
            {
                Email = SecretaryEmail,
                FirstName = SecretaryFirstName,
                LastName = SecretaryLastName
            };

            request.AddJsonBody(body);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        [Test,TestCase(HttpStatusCode.OK, "randomSecretary224243@gmail.com", "Afal", "Barl")]

        public void PUT_UpdateTest(HttpStatusCode expected, string email, string firstName, string lastName)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PUT);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            Secretaries body = new Secretaries()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            request.AddJsonBody(body);
            IRestResponse response = client.Execute(request);

            var actual = response.StatusCode;
            var responseSecretary = JsonConvert.DeserializeObject<Secretaries>(response.Content.ToString());

            Assert.AreEqual(expected, actual);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(email, responseSecretary.Email);
                Assert.AreEqual(firstName, responseSecretary.FirstName);
                Assert.AreEqual(lastName, responseSecretary.LastName);            
            });
        }
    }
}
