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
    public class PUT_UpdateSecretary_NotFoundTest : API_BaseTest
    {
        int SecretaryID { get; set; }

        public PUT_UpdateSecretary_NotFoundTest()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(PUT_UpdateSecretary_NotFoundTest)}");
        }

        [OneTimeSetUp]
        public void PreConditions()
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("GET All Secretaries", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse response = client.Execute(request);

            List<Secretary> secretaries = JsonConvert.DeserializeObject<List<Secretary>>(response.Content.ToString());
            if (!secretaries.Any() || response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Status code: {response.StatusCode} is not {HttpStatusCode.OK}");
            }
            else
            {
                SecretaryID = (secretaries.Last().Id) + 1;
            }
        }

        [Test, TestCase(HttpStatusCode.NotFound)]
        public void PUT_UpdateNotFoundTest(HttpStatusCode expectedStatus)
        {
            RestRequest request = new RestRequest($"secretaries/{SecretaryID}", Method.PUT);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            request.AddJsonBody("{}");

            IRestResponse response = client.Execute(request);

            var actualStatus = response.StatusCode;

            Assert.AreEqual(expectedStatus, actualStatus);
        }
    }
}
