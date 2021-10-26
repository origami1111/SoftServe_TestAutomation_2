using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class POST_ReturnsEventsList_Unauthorized : API_BaseTest
    {
        public POST_ReturnsEventsList_Unauthorized()
        {
            api.log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_Unauthorized)}");
        }

        [Test]
        public void VerifyReturnsEventrsList_StatusCode401()
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath), Method.POST);
            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath)}");
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode");
            api.log.Info($"Expected and actual results is checked");
        }
    }
}