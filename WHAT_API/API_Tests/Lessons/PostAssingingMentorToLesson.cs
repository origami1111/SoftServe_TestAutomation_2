using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_API.Entities.Lessons;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    [AllureNUnit]
    public class PostAssingingMentorToLesson : API_BaseTest
    {
        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin, 6, 5)]
        [TestCase(HttpStatusCode.OK, Role.Secretary, 6, 5)]
        public void AssingingMentorToLesson(HttpStatusCode expectedStatusCode, Role role, int mentorId, int lessonId)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            AssignMentorToLesson assingingMentorRequest = new AssignMentorToLesson().WithMentorId(mentorId).WithLessonId(lessonId);
            var jsonfile = JsonConvert.SerializeObject(assingingMentorRequest);
            var request = InitNewRequest("LessonsAssign", Method.POST, GetAuthenticatorFor(role)).AddJsonBody(jsonfile);
            var response = client.Execute(request);
            var actualCode = response.StatusCode;
            log.Info($"Request is done with {actualCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<AssignedMentorToLesson>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(mentorId, resposneDetaile.MentorId, "Assert mentor id");
                for (int i = 0; i < resposneDetaile.Visits.Count; i++)
                {
                    Assert.AreEqual(resposneDetaile.Visits[i].LessonId, lessonId, "Assert lesson id");
                }
            });
            log.Info($"Expected and actual results is checked");
        }
    }
}

