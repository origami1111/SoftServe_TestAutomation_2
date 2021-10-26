using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class POST_ReturnsEventsList_Valid_ByElement: API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response; 
        private ScheduleGenerator generator = new ScheduleGenerator();
        private LessonsForMentor lessonsForMentor = new LessonsForMentor();
        public POST_ReturnsEventsList_Valid_ByElement()
        {
            api.log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_Valid_ByElement)}");
        }

        /// <summary> Get list of active students using GET request / Student section</summary>
        /// <param name="role"> User role </param>
        /// <returns> LessonsForMentor entity </returns>
        /// 
        private int GetMentorID(ref int mentorID)
        {
            Random random = new Random();
            request = new RestRequest(ReaderUrlsJSON.GetUrlByName("ApiOnlyActiveMentors", api.endpointsPath), Method.GET);
            request.AddHeader("Authorization", api.GetToken(Role.Admin));
            response = APIClient.client.Execute(request);
            List<Mentor> listOfMentors = JsonConvert.DeserializeObject<List<Mentor>>(response.Content.ToString());
            mentorID = listOfMentors.ElementAt(mentorID).Id;
            return mentorID;
        }

        private LessonsForMentor GetProperFilter(Role role)
        {
            LessonsForMentor item = new LessonsForMentor();
            int mentorID = 0;
            bool isValidId = false;
            while (!isValidId)
            {
                mentorID++;
                mentorID = GetMentorID(ref mentorID);
                request = new RestRequest(ReaderUrlsJSON.ByName("ApiMentorsIdLessons", api.endpointsPath), Method.GET);
                request = api.InitNewRequest("ApiMentorsIdLessons", Method.GET, api.GetAuthenticatorFor(role));
                request.AddUrlSegment("id", mentorID.ToString());
                request.AddParameter("id", mentorID);
                response = APIClient.client.Execute(request);
                var listLessonsForMentors = JsonConvert.DeserializeObject<List<LessonsForMentor>>(response.Content);
                if (listLessonsForMentors.Any())
                {
                    isValidId = true;
                    item = listLessonsForMentors.First();
                }
            }
            return item;
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Mentor)]
        public void VerifyReturnsEventrsList_ByMentorId(Role role)
        {
            lessonsForMentor = GetProperFilter(role);
            int expectedMentorId = lessonsForMentor.MentorId;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath), Method.POST);
            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath)}");
            request.AddHeader("Authorization", api.GetToken(role));
            request.AddJsonBody(new { mentorID = expectedMentorId });
            response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(expectedMentorId, item.MentorId, "Presence of an item in the list");
                }
                api.log.Info($"Expected and actual results is checked");
            });
        }
        
        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Mentor)]
        public void VerifyReturnsEventrsList_ByGroupId(Role role)
        {
            lessonsForMentor = GetProperFilter(role);
            int expectedGroupId = lessonsForMentor.StudentGroupId;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath), Method.POST);
            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath)}");
            request.AddHeader("Authorization", api.GetToken(role));
            request.AddJsonBody(new 
            { 
                groupID=expectedGroupId 
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
                    Assert.AreEqual(expectedGroupId, item.StudentGroupId, "Presence of an item in the list");
                }
                api.log.Info($"Expected and actual results is checked");
            });
        }
    }
}