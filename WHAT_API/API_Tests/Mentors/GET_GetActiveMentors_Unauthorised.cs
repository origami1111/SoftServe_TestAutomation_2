using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    class GET_GetActiveMentors_Unauthorised : API_BaseTest
    {
        [Test]
        public void VerifyGetActiveMentors_Unauthorised()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetActiveMentors_Unauthorised)}");

            var endpoint = "ApiOnlyActiveMentors";
            var request = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.GET);
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
