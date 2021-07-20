using NLog;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class POST_ReturnsEventsList_Unauthorized : API_BaseTest
    {
        public POST_ReturnsEventsList_Unauthorized()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_Unauthorized)}");
        }

        [Test]
        public void VerifyReturnsEventrsList_StatusCode401()
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode");
            log.Info($"Expected and actual results is checked");
        }
    }
}