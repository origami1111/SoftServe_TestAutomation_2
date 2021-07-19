using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.POST_ReturnsEventrsList
{
    [TestFixture]
    public class POST_ReturnsEventrsList_Valid_ByElement: API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response; 
        private ScheduleGenerator generator = new ScheduleGenerator();
        private ScheduleFilterGenerator filterGenerator = new ScheduleFilterGenerator();
        private LessonsForMentor lessonsForMentor = new LessonsForMentor();
        public POST_ReturnsEventrsList_Valid_ByElement()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventrsList_Valid_ByElement)}");
        }
        public LessonsForMentor GetProperFilter(Role role)
        {
            LessonsForMentor item = new LessonsForMentor();
            bool isValidId = false;
            while (!isValidId)
            {
                int mentorID = generator.GetMentorID();
                request = new RestRequest(ReaderUrlsJSON.ByName("ApiMentorsIdLessons", endpointsPath), Method.GET);
                request = InitNewRequest("ApiMentorsIdLessons", Method.GET, GetAuthenticatorFor(role));
                request.AddUrlSegment("id", mentorID.ToString());
                request.AddParameter("id", mentorID);
                response = client.Execute(request);
                var listLessonsForMentors = JsonConvert.DeserializeObject<List<LessonsForMentor>>(response.Content);
                if (listLessonsForMentors.Any())
                {
                    isValidId = true;
                    item = listLessonsForMentors.First();
                }
            }
            return item;
        }

        public StudentsGroup GetStudentsGroup(int studentGroupId, Role role)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsGroupId", endpointsPath), Method.GET);
            request = InitNewRequest("ApiStudentsGroupId", Method.GET, GetAuthenticatorFor(role));
            request.AddUrlSegment("id", studentGroupId.ToString());
            request.AddParameter("id", studentGroupId);
            IRestResponse response = client.Execute(request);
            var listLessonsForMentors = JsonConvert.DeserializeObject<StudentsGroup>(response.Content);
            return listLessonsForMentors;
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Mentor)]
        public void VerifyReturnsEventrsList_ByMentorId(Role role)
        {
            lessonsForMentor = GetProperFilter(role);
            int expectedMentorId = lessonsForMentor.MentorId;
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(role));
            request.AddJsonBody(new { mentorID = expectedMentorId });
            response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(expectedMentorId, item.MentorId, "Presence of an item in the list");
                }
                log.Info($"Expected and actual results is checked");
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
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(role));
            request.AddJsonBody(new 
            { 
                groupID=expectedGroupId 
            });
            response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<ScheduledEvent>>(response.Content);
            Assert.That(events.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(expectedGroupId, item.StudentGroupId, "Presence of an item in the list");
                }
                log.Info($"Expected and actual results is checked");
            });
        }

    }
}