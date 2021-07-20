using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class POST_ReturnsEventsList_Valid_FullBody : API_BaseTest
    {
        private IRestResponse response;
        private RestRequest request;
        private ScheduleGenerator generator = new ScheduleGenerator();

        public POST_ReturnsEventsList_Valid_FullBody()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_Valid_FullBody)}");
        }

        private static IEnumerable<int[]> FilterRulesSources()
        {
            yield return new int[] { 9, 1, 1, 1};
            yield return new int[] { 3, 12, 19, 12 };
            yield return new int[] { 8, 6, 13, 6 };
            yield return new int[] { 12, 9, 7, 9 };
            yield return new int[] { 12, 9, 7, 9 };
        }

        private static IEnumerable<Role> FilterRoleSources()
        {
            yield return Role.Admin;
            yield return Role.Mentor;
            yield return Role.Secretary;
            yield return Role.Student;
        }

        [Test]
        public void VerifyReturnsEventrsList_FullBody_Valid([ValueSource(nameof(FilterRulesSources))] int[] filter,
            [ValueSource(nameof(FilterRoleSources))] Role role)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(role));
            request.AddJsonBody(new
            {
                mentorID = filter[(int)FilterIndex.MentorId],
                groupID = filter[(int)FilterIndex.GroupId],
                themeID = filter[(int)FilterIndex.ThemeId],
                eventOccurrenceID = filter[(int)FilterIndex.EventOccurrenceId],
            });
            response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(filter[(int)FilterIndex.MentorId], item.MentorId, "Presence of MentorId");
                    Assert.AreEqual(filter[(int)FilterIndex.GroupId], item.StudentGroupId, "Presence of StudentGroupId");
                    Assert.AreEqual(filter[(int)FilterIndex.ThemeId], item.ThemeId, "Presence of ThemeId");
                    Assert.AreEqual(filter[(int)FilterIndex.EventOccurrenceId], item.EventOccuranceId, "Presence of EventOccuranceId");
                }
                log.Info($"Expected and actual results is checked");
            });
        }

        [Test]
        public void VerifyReturnsEventrsList_FullBody_Inalid([ValueSource(nameof(FilterRulesSources))] int[] filter)
        {
            var role = Role.Unassigned;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(role));
            request.AddJsonBody(new
            {
                mentorID = filter[(int)FilterIndex.MentorId],
                groupID = filter[(int)FilterIndex.GroupId],
                themeID = filter[(int)FilterIndex.ThemeId],
                eventOccurrenceID = filter[(int)FilterIndex.EventOccurrenceId],
            });
            response = client.Execute(request);
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            log.Info($"Request is done with {response.StatusCode} StatusCode");
        }
    }
}