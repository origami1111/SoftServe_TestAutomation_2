using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class POST_ReturnsEventrsList_Unauthorized : API_BaseTest
    {
        [Test]
        public void VerifyReturnsEventrsList_StatusCode401()
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode");
            log.Info($"Request is done with {response.StatusCode} StatusCode");
        }
    }
}