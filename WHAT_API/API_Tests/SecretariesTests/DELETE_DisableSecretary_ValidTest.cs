using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class DELETE_DisableSecretary_ValidTest: API_BaseTest
    {
        Random random = new Random();

        int SecretaryID { get; set; }

        [OneTimeSetUp]
        public void PreConditions()
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("GET Active Secretaries", endpointsPath), Method.GET);
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
            }
        }

        [OneTimeTearDown]
        public void PostConditions()
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PATCH);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        [Test, TestCase(HttpStatusCode.OK, "true")]

        public void DELETE_DisableTest(HttpStatusCode expectedStatus, string expectedResponse)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.DELETE);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            var actual = response.StatusCode;
            string responseActual = response.Content;

            Assert.AreEqual(expectedStatus, actual);

            Assert.AreEqual(expectedResponse, responseActual);
        }
    }
}
