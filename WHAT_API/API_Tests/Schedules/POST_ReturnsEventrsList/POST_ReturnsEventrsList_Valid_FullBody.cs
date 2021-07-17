using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.POST_ReturnsEventrsList
{
    [TestFixture]
    public class POST_ReturnsEventrsList_Successful: API_BaseTest
    {
        private ScheduleGenerator generator = new ScheduleGenerator();
        public POST_ReturnsEventrsList_Successful()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventrsList_Successful)}");
        }

        [Test]
        [TestCase(/*9, 1, 1, 1, 1*/Role.Admin)]
        [TestCase(/*9, 1, 1, 1, 1*/Role.Secretar)]
        [TestCase(/*9, 1, 1, 1, 1*/Role.Student)]
        [TestCase(/*9, 1, 1, 1, 1*/Role.Mentor)]
        public void VerifyReturnsEventrsList_FullBody(/*int mentorId, int groupID, int themeIDint studentAccountID, int eventOccurrenceID,*/Role user)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(user));
            int mentorID = generator.GetMentorID();
            int themeID = generator.GetThemeID();
            int groupID = generator.GetStudentsGroupID();
            request.AddJsonBody(new { mentorID, groupID,themeID});
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(mentorID, item.MentorId, "Presence of MentorId");
                    Assert.AreEqual(groupID, item.StudentGroupId, "Presence of StudentGroupId");
                    Assert.AreEqual(themeID, item.ThemeId, "Presence of ThemeId");
                    //Assert.AreEqual(studentAccountID, item.Id, "Presence of studentAccountID");
                    //Assert.AreEqual(eventOccurrenceID, item.EventOccuranceId, "Presence of EventOccuranceId");
                }
                log.Info($"Expected and actual results is checked");
            });
        }
    }
}