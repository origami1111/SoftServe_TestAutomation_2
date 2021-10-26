using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class POST_ReturnsEventsList_Valid_FullBody : API_BaseTest
    {
        private IRestResponse response;
        private RestRequest request;
        private ScheduleGenerator generator = new ScheduleGenerator();

        public POST_ReturnsEventsList_Valid_FullBody()
        {
            api.log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_Valid_FullBody)}");
        }

        /// <summary> Return set values with filter for POST request</summary>
        private static IEnumerable<int[]> FilterRulesSources()
        {
            yield return new int[] { 9, 1, 1, 1};
            yield return new int[] { 3, 12, 19, 12 };
            yield return new int[] { 8, 6, 13, 6 };
            yield return new int[] { 12, 9, 7, 9 };
            yield return new int[] { 12, 9, 7, 9 };
        }

        /// <summary> Return role list for POST request</summary>
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
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath), Method.POST);
            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath)}");
            request.AddHeader("Authorization", api.GetToken(role));
            request.AddJsonBody(new
            {
                mentorID = filter[(int)FilterIndex.MentorId],
                groupID = filter[(int)FilterIndex.GroupId],
                themeID = filter[(int)FilterIndex.ThemeId],
                eventOccurrenceID = filter[(int)FilterIndex.EventOccurrenceId],
            });
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
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
                api.log.Info($"Expected and actual results is checked");
            });
        }

        [Test]
        public void VerifyReturnsEventrsList_FullBody_Invalid([ValueSource(nameof(FilterRulesSources))] int[] filter)
        {
            var role = Role.Unassigned;
            try
            {
                request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath), Method.POST);
                api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath)}");
                request.AddHeader("Authorization", api.GetToken(role));
                Assert.Fail();
                api.log.Fatal("Not correct token, user is valid");
            }
            catch (Exception)
            {
                Assert.Pass();
                api.log.Info("Exception is cought and correctly handled");
            }
        }
    }
}