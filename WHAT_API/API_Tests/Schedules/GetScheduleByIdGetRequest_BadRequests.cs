using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    class GetScheduleByIdGetRequest_BadRequests : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        [Test]
        [TestCase(HttpStatusCode.NotFound, Role.Admin, -10)]
        [TestCase(HttpStatusCode.NotFound, Role.Admin, 0)]
        [TestCase(HttpStatusCode.NotFound, Role.Admin, 100000)]
        public void GetScheduleWithStatusCodeError(HttpStatusCode expectedStatusCode, Role role, int id)
        {
            request = new RestRequest(ReaderUrlsJSON.GetUrlByName("ApiSchedules-id", endpointsPath) + id, Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }
    }
}
