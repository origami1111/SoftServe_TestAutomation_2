using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class POST_AddShedule_BadRequestTest : API_BaseTest
    {
        private CreateSchedule schedule;
        private RestRequest request;
        private IRestResponse response;

        [OneTimeSetUp]
        public void PreConditions()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedules", endpointsPath), Method.POST);
            request.AddHeader("Authorization", GetToken(Role.Admin));
        }

        [Test, TestCase(HttpStatusCode.BadRequest, "wrongData")]

        public void POST_MissingData(HttpStatusCode expected, string data)
        {
            request.AddJsonBody(data);
            response = client.Execute(request);

            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test, TestCase(HttpStatusCode.BadRequest)]

        public void POST_WrongDate(HttpStatusCode expected)
        {
            List<DayOfWeek> list = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Friday };
            DateTime startDate = new DateTime(2022, 1, 2, 13, 27, 09).ToUniversalTime();
            DateTime finishDate = new DateTime(2021, 7, 7, 15, 27, 09).ToUniversalTime();

            schedule = new ScheduleGenerator()
                           .GenerateShedule(PatternType.Daily, 3, list, startDate, finishDate, 1, 3, 4);

            request.AddJsonBody(schedule);
            response = client.Execute(request);

            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
    }
}
