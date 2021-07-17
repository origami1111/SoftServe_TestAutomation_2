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
    public class POST_ReturnsEventrsList_Valid_ByElement: API_BaseTest
    {
        private ScheduleGenerator generator = new ScheduleGenerator();
        public POST_ReturnsEventrsList_Valid_ByElement()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventrsList_Valid_ByElement)}");
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        [TestCase(Role.Student)]
        [TestCase(Role.Mentor)]
        public void VerifyReturnsEventrsList_ByMentorId(Role user)
        {
            int mentorID = generator.GetMentorID();
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(user));
            request.AddJsonBody(new{mentorID});
            IRestResponse response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(mentorID, item.MentorId, "Presence of an item in the list");
                }
                log.Info($"Expected and actual results is checked");
            });
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        [TestCase(Role.Student)]
        [TestCase(Role.Mentor)]
        public void VerifyReturnsEventrsList_ByThemeId(Role user)
        {
            int themeID = generator.GetThemeID();
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(user));
            request.AddJsonBody(new { themeID });
            IRestResponse response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(themeID, item.ThemeId, "Presence of an item in the list");
                }
                log.Info($"Expected and actual results is checked");
            });
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        [TestCase(Role.Student)]
        [TestCase(Role.Mentor)]
        public void VerifyReturnsEventrsList_ByGroupId(Role user)
        {
            int groupID = generator.GetStudentsGroupID();
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(user));
            request.AddJsonBody(new { groupID });
            IRestResponse response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(groupID, item.StudentGroupId, "Presence of an item in the list");
                }
                log.Info($"Expected and actual results is checked");
            });
        }
    }
}