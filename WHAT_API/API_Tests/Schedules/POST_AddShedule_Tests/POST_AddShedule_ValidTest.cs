using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using WHAT_Utilities;
using NLog;

namespace WHAT_API
{
    [TestFixture]
    public class POST_AddShedule_ValidTest : API_BaseTest
    {
        private CreateSchedule schedule;
        private RestRequest request;
        private IRestResponse response;
        long? occurrenceID;

        public POST_AddShedule_ValidTest()
        {
            log = LogManager.GetLogger($"Schedules/{nameof(POST_AddShedule_ValidTest)}");
        }

        [OneTimeSetUp]
        public void PreConditions()
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedules", endpointsPath), Method.POST);
            request.AddHeader("Authorization", GetToken(Role.Admin));

            schedule = new ScheduleGenerator()
                           .GenerateShedule();

            request.AddJsonBody(schedule);
            response = client.Execute(request);
        }

        [OneTimeTearDown]
        public void PostConditions()
        {
            RestRequest deleteRequest = new RestRequest($"schedules/{occurrenceID}", Method.DELETE);
            deleteRequest.AddHeader("Authorization", GetToken(Role.Admin));

            IRestResponse deleteResponse = client.Execute(deleteRequest);

            if (deleteResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Status code: {deleteResponse.StatusCode} is not {HttpStatusCode.OK}");
            }
        }

        [Test, TestCase(HttpStatusCode.OK)]
        public void POST_ValidData(HttpStatusCode expectedStatus)
        {
            var actualStatus = response.StatusCode;
            string stream = response.Content;
            var jsonSchedule = JsonConvert.DeserializeObject<EventOccurrence>(stream);
            occurrenceID = jsonSchedule.Id;

            Assert.AreEqual(expectedStatus, actualStatus);

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
                    Assert.LessOrEqual(item.EventFinish, schedule.Range.FinishDate);
                    Assert.GreaterOrEqual(item.EventStart, schedule.Range.StartDate);
                }
            });
        }

    }
}
