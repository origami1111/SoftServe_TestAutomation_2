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
    class GetScheduleByIdGetRequest_BadRequests : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public GetScheduleByIdGetRequest_BadRequests()
        {
            api.log = LogManager.GetLogger($"Schedule/{nameof(GetScheduleByIdGetRequest_BadRequests)}");
        }

        [Test]
        [TestCase(HttpStatusCode.NotFound, Role.Admin, -10)]
        [TestCase(HttpStatusCode.NotFound, Role.Admin, 0)]
        [TestCase(HttpStatusCode.NotFound, Role.Admin, 100000)]
        public void GetScheduleWithStatusCodeError(HttpStatusCode expectedStatusCode, Role role, int id)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            request = api.InitNewRequest("ApiSchedulesById-id", Method.GET, authenticator);
            request.AddUrlSegment("id", id.ToString());

            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSchedulesById-id", api.endpointsPath)}");
            response = api.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
        }

    }
}
