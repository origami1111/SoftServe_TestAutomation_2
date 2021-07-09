using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    class GetScheduleByIdGetRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private long? id;
        private EventOccurrence expected;

        //[SetUp]
        //public void Setup()
        //{
        //    int id = 1;
        //    request = new RestRequest($"schedules/{id}", Method.GET);
        //    //request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedules-id", endpointsPath), Method.GET);
        //}

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


        [OneTimeSetUp]
        public void PreCondition()
        {
            // Create schedule by POST method
            request = new RestRequest("schedules", Method.POST);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            ScheduleGenerator scheduleGenerator = new ScheduleGenerator();
            Schedule schedule = scheduleGenerator.GenerateSchedule();

            request.AddJsonBody(schedule);
            response = client.Execute(request);
            string stream = response.Content;
            expected = JsonConvert.DeserializeObject<EventOccurrence>(stream);
            id = expected.Id;
        }

        [OneTimeTearDown]
        public void PostCondtion()
        {
            // Delete created schedule
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        public void GetScheduleWithStatusCode200(HttpStatusCode expectedStatusCode)
        {
            request = new RestRequest($"schedules/{id}", Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string streamActual = response.Content;

            EventOccurrence actual = JsonConvert.DeserializeObject<EventOccurrence>(streamActual);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
            });
        }

    }
}
