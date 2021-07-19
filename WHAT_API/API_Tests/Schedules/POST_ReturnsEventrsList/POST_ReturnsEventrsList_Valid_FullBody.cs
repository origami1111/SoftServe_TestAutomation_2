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
    public class POST_ReturnsEventrsList_Valid_FullBody : API_BaseTest
    {
        private IRestResponse response;
        private RestRequest request;

        private ScheduleGenerator generator = new ScheduleGenerator();

        public POST_ReturnsEventrsList_Valid_FullBody()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventrsList_Valid_FullBody)}");
        }
        private static IEnumerable<TestCaseData> FilterRulesSources
        {
            get
            {
                yield return new TestCaseData(9, 1, 1, 1, Role.Admin);
                yield return new TestCaseData(9, 1, 1, 1, Role.Student);
                yield return new TestCaseData(9, 1, 1, 1, Role.Mentor);
                yield return new TestCaseData(9, 1, 1, 1, Role.Secretary);
                yield return new TestCaseData(3, 12, 19, 12, Role.Admin);
                yield return new TestCaseData(3, 12, 19, 12, Role.Student);
                yield return new TestCaseData(3, 12, 19, 12, Role.Mentor);
                yield return new TestCaseData(3, 12, 19, 12, Role.Secretary); 
                yield return new TestCaseData(8, 6, 13, 6, Role.Admin);
                yield return new TestCaseData(8, 6, 13, 6, Role.Student);
                yield return new TestCaseData(8, 6, 13, 6, Role.Mentor);
                yield return new TestCaseData(8, 6, 13, 6, Role.Secretary);
            }
        }

        [Test]
        [TestCaseSource(nameof(FilterRulesSources))]
        public void VerifyReturnsEventrsList_FullBody(int mentorID, int groupID, int themeID,
            int eventOccurrenceID, Role role)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(role));
            request.AddJsonBody(new
            {
                mentorID = mentorID,
                groupID = groupID,
                themeID = themeID,
                eventOccurrenceID = eventOccurrenceID,
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
                    Assert.AreEqual(mentorID, item.MentorId, "Presence of MentorId");
                    Assert.AreEqual(groupID, item.StudentGroupId, "Presence of StudentGroupId");
                    Assert.AreEqual(themeID, item.ThemeId, "Presence of ThemeId");
                    Assert.AreEqual(eventOccurrenceID, item.EventOccuranceId, "Presence of EventOccuranceId");
                }
                log.Info($"Expected and actual results is checked");
            });
        }
    }
}