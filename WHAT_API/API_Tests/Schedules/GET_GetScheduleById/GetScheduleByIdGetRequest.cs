using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    class GetScheduleByIdGetRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private EventOccurrence expected;

        public GetScheduleByIdGetRequest()
        {
            api.log = LogManager.GetLogger($"Schedule/{nameof(GetScheduleByIdGetRequest)}");
        }

        /// <summary>
        /// Create schedule by POST method
        /// </summary>
        [OneTimeSetUp]
        public void PreCondition()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            request = api.InitNewRequest("ApiSchedules", Method.POST, authenticator);

            ScheduleGenerator scheduleGenerator = new ScheduleGenerator();
            CreateSchedule schedule = scheduleGenerator.GenerateShedule();

            request.AddJsonBody(schedule);

            response = api.Execute(request);
            expected = JsonConvert.DeserializeObject<EventOccurrence>(response.Content);
        }

        /// <summary>
        /// Delete created schedule by DELETE method
        /// </summary>
        [OneTimeTearDown]
        public void PostCondition()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            request = api.InitNewRequest("ApiSchedulesEventOccurenceID-eventOccurenceID", Method.DELETE, authenticator);
            request.AddUrlSegment("eventOccurenceID", expected.Id.ToString());

            response = api.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Failed to delete schedule");
            }
        }

        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin)]
        [TestCase(HttpStatusCode.OK, Role.Secretary)]
        public void GetScheduleWithStatusCode200(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            request = api.InitNewRequest("ApiSchedulesById-id", Method.GET, authenticator);
            request.AddUrlSegment("id", expected.Id.ToString());

            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSchedulesById-id", api.endpointsPath)}");
            response = api.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
            api.log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            EventOccurrence actual = JsonConvert.DeserializeObject<EventOccurrence>(response.Content);
            actual.Events = actual.Events.OrderBy(ev => ev.EventStart).ToList();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Id, actual.Id, "Schedule id");
                Assert.AreEqual(expected.StudentGroupId, actual.StudentGroupId, "Student group id");
                Assert.AreEqual(expected.EventStart, actual.EventStart, "Event start");
                Assert.AreEqual(expected.EventFinish, actual.EventFinish, "Event finish");
                Assert.AreEqual(expected.Pattern, actual.Pattern, "Pattern");
                Assert.AreEqual(expected.Storage, actual.Storage, "Storage");

                Assert.AreEqual(expected.Events.Count, actual.Events.Count, "Events count");
                for (int i = 0; i < expected.Events.Count; i++)
                {
                    Assert.AreEqual(expected.Events[i].EventOccuranceId, actual.Events[i].EventOccuranceId, "Event occurance id");
                    Assert.AreEqual(expected.Events[i].StudentGroupId, actual.Events[i].StudentGroupId, "Student group id");
                    Assert.AreEqual(expected.Events[i].ThemeId, actual.Events[i].ThemeId, "Theme id");
                    Assert.AreEqual(expected.Events[i].MentorId, actual.Events[i].MentorId, "Mentor id");
                    Assert.AreEqual(expected.Events[i].LessonId, actual.Events[i].LessonId, "Lesson id");
                    Assert.AreEqual(expected.Events[i].EventStart, actual.Events[i].EventStart, "Event start");
                    Assert.AreEqual(expected.Events[i].EventFinish, actual.Events[i].EventFinish, "Event finish");
                }
            });
            api.log.Info($"Expected and actual result is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Forbidden, Role.Mentor)]
        [TestCase(HttpStatusCode.Forbidden, Role.Student)]
        public void GetScheduleWithStatusCode403(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            request = api.InitNewRequest("ApiSchedulesById-id", Method.GET, authenticator);
            request.AddUrlSegment("id", expected.Id.ToString());

            api.log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiSchedulesById-id", api.endpointsPath)}");
            response = api.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
        }

    }
}
