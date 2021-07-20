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
    public class PATCH_EnableSecretary_ValidTest: API_BaseTest
    {
        Random random = new Random();

        int SecretaryID { get; set; }

        [OneTimeSetUp]
        public void PreConditions()
        {
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("GET Active Secretaries", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(getRequest);

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

            RestRequest deleteRequest = new RestRequest($"secretaries/{SecretaryID}", Method.DELETE);
            deleteRequest.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse deleteResponse = client.Execute(deleteRequest);

            if (deleteResponse.StatusCode != HttpStatusCode.OK && deleteResponse.Content.ToString() != "true")
            {
                throw new Exception();
            }
        }

        [Test, TestCase(HttpStatusCode.OK)]

        public void PATCH_EnableTest(HttpStatusCode expectedStatus)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PATCH);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            var actual = response.StatusCode;

            Assert.AreEqual(expectedStatus, actual);
        }
    }
}
