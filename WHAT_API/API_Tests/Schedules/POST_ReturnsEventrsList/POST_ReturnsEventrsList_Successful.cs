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
    public class POST_ReturnsEventrsList_Successful: API_BaseTest
    {
        [Test]
        public void VerifyReturnsEventrsList_ByMentorId([Random(1,20,2)] int mentorID, 
            [Values(Role.Admin, Role.Secretar, Role.Student, Role.Mentor)] Role user)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(user));
            request.AddJsonBody(new
            {
                mentorID = mentorID
            });
            IRestResponse response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(mentorID, item.MentorId, "Presence of an item in the list");
                }
                log.Info($"{events.Count} elements is checked");
            });
        }

        [Test]
        public void VerifyReturnsEventrsList_FullBody
           ([Values(9)] int mentorID,
            [Values(1)] int groupID,
            [Values(1)] int themeID,
            [Values(1)] int studentAccountID,
            [Values(1)] int eventOccurrenceID,
            [Values(Role.Admin, Role.Secretar, Role.Student, Role.Mentor)] Role user)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(user));
            request.AddJsonBody(new
            {
                mentorID = mentorID,
                groupID = groupID,
                themeID = themeID,
                studentAccountID = studentAccountID,
                eventOccurrenceID = eventOccurrenceID,
            });
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(mentorID, item.MentorId, "Presence of MentorId");
                    Assert.AreEqual(groupID, item.StudentGroupId, "Presence of StudentGroupId");
                    Assert.AreEqual(themeID, item.ThemeId, "Presence of ThemeId");
                    Assert.AreEqual(studentAccountID, item.Id, "Presence of studentAccountID");
                    Assert.AreEqual(eventOccurrenceID, item.EventOccuranceId, "Presence of EventOccuranceId");
                }
                log.Info($"{events.Count} elements is checked");
            });
        }
    }
}