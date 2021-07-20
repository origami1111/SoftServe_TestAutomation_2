using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    class GetScheduleByIdGetRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private EventOccurrence expected;

        /// <summary>
        /// Create schedule by POST method
        /// </summary>
        [OneTimeSetUp]
        public void PreCondition()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            request = InitNewRequest("ApiSchedules", Method.POST, authenticator);

            ScheduleGenerator scheduleGenerator = new ScheduleGenerator();
            CreateSchedule schedule = scheduleGenerator.GenerateShedule();

            request.AddJsonBody(schedule);

            expected = Execute<EventOccurrence>(request);
        }

        /// <summary>
        /// Delete created schedule by DELETE method
        /// </summary>
        [OneTimeTearDown]
        public void PostCondition()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            request = InitNewRequest("ApiSchedulesEventOccurenceID-eventOccurenceID", Method.DELETE, authenticator);
            request.AddUrlSegment("eventOccurenceID", expected.Id.ToString());

            response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin)]
        [TestCase(HttpStatusCode.OK, Role.Secretary)]
        public void GetScheduleWithStatusCode200(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            request = InitNewRequest("ApiSchedulesById-id", Method.GET, authenticator);
            request.AddUrlSegment("id", expected.Id.ToString());
            
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSchedulesById-id", endpointsPath)}");
            response = client.Execute(request);
            
            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            string json = response.Content;
            EventOccurrence actual = JsonConvert.DeserializeObject<EventOccurrence>(json);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
            });
            log.Info($"Expected and actual result is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Forbidden, Role.Mentor)]
        [TestCase(HttpStatusCode.Forbidden, Role.Student)]
        public void GetScheduleWithStatusCode403(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            request = InitNewRequest("ApiSchedulesById-id", Method.GET, authenticator);
            request.AddUrlSegment("id", expected.Id.ToString());

            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSchedulesById-id", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

    }
}
