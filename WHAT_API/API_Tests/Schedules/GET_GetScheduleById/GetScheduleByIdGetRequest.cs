using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
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


        public class Schedule
        {
            public Pattern Pattern { get; set; }
            public Range Range { get; set; }
            public Context Context { get; set; }
        }

        public class Pattern
        {
            public int Type { get; set; }
            public int Interval { get; set; }
            public List<int> DaysOfWeek { get; set; }
            public int Index { get; set; }
            public List<int> Dates { get; set; }
        }

        public class Range
        {
            public DateTime StartDate { get; set; }
            public DateTime FinishDate { get; set; }
        }

        public class Context
        {
            public int MentorId { get; set; }
            public int ThemeId { get; set; }
            public int GroupId { get; set; }
        }

        public class ScheduleGenerator
        {
            private Schedule schedule = new Schedule();

            public Schedule GenerateSchedule()
            {
                schedule.Pattern = new Pattern()
                {
                    Type = 3,
                    Interval = 1,
                    DaysOfWeek = new List<int>() { 4, 5 },
                    Index = 2,
                    Dates = null
                };
                schedule.Range = new Range()
                {
                    StartDate = Convert.ToDateTime("2021-07-01T10:00:00"),
                    FinishDate = Convert.ToDateTime("2021-08-31T11:00:00")
                };
                schedule.Context = new Context()
                {
                    MentorId = 5,
                    ThemeId = 6,
                    GroupId = 2
                };

                return schedule;
            }
        }


        /// <summary>
        /// Create schedule by POST method
        /// </summary>
        [OneTimeSetUp]
        public void PreCondition()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            request = InitNewRequest("ApiSchedules", Method.POST, authenticator);

            ScheduleGenerator scheduleGenerator = new ScheduleGenerator();
            Schedule schedule = scheduleGenerator.GenerateSchedule();

            //ScheduleGenerator scheduleGenerator = new ScheduleGenerator();
            //CreateSchedule schedule = scheduleGenerator.GenerateShedule();

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
            log.Info($"Request is done with {actualStatusCode} StatusCode");

            string json = response.Content;
            EventOccurrence actual = JsonConvert.DeserializeObject<EventOccurrence>(json);

            Assert.Multiple(() =>
            {
                //Assert.AreEqual(expected, actual);

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.StudentGroupId, actual.StudentGroupId);
                Assert.AreEqual(expected.EventStart, actual.EventStart);
                Assert.AreEqual(expected.EventFinish, actual.EventFinish);
                Assert.AreEqual(expected.Pattern, actual.Pattern);
                Assert.AreEqual(expected.Storage, actual.Storage);

                Assert.AreEqual(expected.Events.Count, actual.Events.Count);
                for (int i = 0; i < expected.Events.Count; i++)
                {
                    Assert.AreEqual(expected.Events[i].EventOccuranceId, actual.Events[i].EventOccuranceId);
                    Assert.AreEqual(expected.Events[i].StudentGroupId, actual.Events[i].StudentGroupId);
                    Assert.AreEqual(expected.Events[i].ThemeId, actual.Events[i].ThemeId);
                    Assert.AreEqual(expected.Events[i].MentorId, actual.Events[i].MentorId);
                    Assert.AreEqual(expected.Events[i].LessonId, actual.Events[i].LessonId);
                    Assert.AreEqual(expected.Events[i].EventStart, actual.Events[i].EventStart);
                    Assert.AreEqual(expected.Events[i].EventFinish, actual.Events[i].EventFinish);
                }

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
            log.Info($"Request is done with {actualStatusCode} StatusCode");

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

    }
}
