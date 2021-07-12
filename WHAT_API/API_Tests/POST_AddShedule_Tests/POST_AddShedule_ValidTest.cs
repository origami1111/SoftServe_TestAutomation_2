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
    public class POST_AddShedule_ValidTest : API_BaseTest
    {
        private CreateSchedule schedule;
        private RestRequest request;
        private IRestResponse response;

        [OneTimeSetUp]
        public void PreConditions()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedules", endpointsPath), Method.POST);
            request.AddHeader("Authorization", GetToken(Role.Admin));


            schedule = new SheduleGenerator()
                           .GenerateShedule();

            request.AddJsonBody(schedule);
            response = client.Execute(request);
        }

        [Test, TestCase(HttpStatusCode.OK)]

        public void POST_ValidData(HttpStatusCode expected)
        {
            var actual = response.StatusCode;
            string stream = response.Content;
            var jsonSchedule = JsonConvert.DeserializeObject<EventOccurrence>(stream);

            Assert.AreEqual(expected, actual);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(schedule.Pattern.Type, jsonSchedule.Pattern);
                Assert.AreEqual(schedule.Context.GroupID, jsonSchedule.StudentGroupId);
                Assert.AreEqual(schedule.Range.StartDate, jsonSchedule.EventStart);
                Assert.AreEqual(schedule.Range.FinishDate, jsonSchedule.EventFinish);

                foreach (var item in jsonSchedule.Events)
                {
                    Assert.AreEqual(item.MentorId, schedule.Context.MentorID);
                    Assert.AreEqual(item.StudentGroupId, schedule.Context.GroupID);
                    Assert.AreEqual(item.ThemeId, schedule.Context.ThemeID);
                    Assert.LessOrEqual(item.EventFinish,schedule.Range.FinishDate);
                    Assert.GreaterOrEqual(item.EventStart,schedule.Range.StartDate);
                }
            });
        }

    }
}
