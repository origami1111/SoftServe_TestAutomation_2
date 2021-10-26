using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_API.Entities;
using WHAT_API.Entities.Lessons;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    [AllureNUnit]
    public class PostAssingingMentorToLesson : API_BaseTest
    {
        [TestCaseSource(typeof(TestCase), nameof(TestCase.ValidAssingingMentorToLesson))]
        public void AssingingMentorToLesson(HttpStatusCode expectedStatusCode, Role role)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(PostAssingingMentorToLesson)}");
            var mentorRequest = api.InitNewRequest("ApiOnlyActiveMentors",Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var mentorResponse = APIClient.client.Execute(mentorRequest);
            int mentorId = JsonConvert.DeserializeObject<List<Mentor>>(mentorResponse.Content).FirstOrDefault().Id;
            var lessonRequest = api.InitNewRequest("Lessons", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var lessonResponse = APIClient.client.Execute(lessonRequest);
            int lessonId = JsonConvert.DeserializeObject<List<Lesson>>(lessonResponse.Content).FirstOrDefault().Id;
            AssignMentorToLesson assingingMentorRequest = new AssignMentorToLesson()
                .WithMentorId(mentorId)
                .WithLessonId(lessonId);
            var jsonfile = JsonConvert.SerializeObject(assingingMentorRequest);
            var request = api.InitNewRequest("LessonsAssign", Method.POST, api.GetAuthenticatorFor(role)).AddJsonBody(jsonfile);
            var response = APIClient.client.Execute(request);
            var actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<AssignedMentorToLesson>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(mentorId, resposneDetaile.MentorId, "Assert mentor id");
                foreach (var visit in resposneDetaile.Visits)
                {
                    Assert.AreEqual(visit.LessonId, lessonId, "Assert lesson id");
                }
            });
            api.log.Info($"Expected and actual results is checked");
        }
        
        [TestCaseSource(typeof(TestCase), nameof(TestCase.ForbiddenAssingingMentorToLesson))]
        public void VerifyForbiddenStatusCode(HttpStatusCode expectedStatusCode, Role role)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(PostAssingingMentorToLesson)}");
            var mentorRequest = api.InitNewRequest("ApiOnlyActiveMentors", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var mentorResponse = APIClient.client.Execute(mentorRequest);
            int mentorId = JsonConvert.DeserializeObject<List<Mentor>>(mentorResponse.Content).FirstOrDefault().Id;
            var lessonRequest = api.InitNewRequest("Lessons", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var lessonResponse = APIClient.client.Execute(lessonRequest);
            int lessonId = JsonConvert.DeserializeObject<List<Lesson>>(lessonResponse.Content).FirstOrDefault().Id;
            AssignMentorToLesson assingingMentorRequest = new AssignMentorToLesson()
                .WithMentorId(mentorId)
                .WithLessonId(lessonId);
            var jsonfile = JsonConvert.SerializeObject(assingingMentorRequest);
            var request = api.InitNewRequest("LessonsAssign", Method.POST, api.GetAuthenticatorFor(role)).AddJsonBody(jsonfile);
            var response = APIClient.client.Execute(request);
            var actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            api.log.Info($"Expected and actual results is checked");
        }
    }
}

