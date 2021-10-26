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
    public class GetListOfAllLessons : API_BaseTest
    {
        [TestCaseSource(typeof(TestCase), nameof(TestCase.ValidForListOfAllLessons))]
        public void GetsListOfAllLessons(HttpStatusCode expectedStatusCode,Role role, string thema, int mentorId, string date, int mark, bool presense, string comment, int increment)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(GetListOfAllLessons)}");
            var request = api.InitNewRequest("Lessons", Method.GET, api.GetAuthenticatorFor(role));
            var response = APIClient.client.Execute(request);
            var actualCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualCode, "Assert status code");
            int beforeCount = JsonConvert.DeserializeObject<List<Lesson>>(response.Content).Count;
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");

            var getRequest = api.InitNewRequest("ApiStudentsGroup", Method.GET, api.GetAuthenticatorFor(role));
            var getResponse = APIClient.client.Execute(getRequest);
            var responseDetail = JsonConvert.DeserializeObject<List<StudentGroup>>(getResponse.Content);
            var studentGroup = responseDetail.FirstOrDefault();
            List<CreateVisit> lessonVisits = new List<CreateVisit>();
            for (int i = 0; i < studentGroup.StudentIds.Count; i++)
            {
                lessonVisits
                    .Add(new CreateVisit()
                    .WithStudentId(studentGroup.StudentIds[i])
                    .WithStudentMark(mark)
                    .WithPresence(presense)
                    .WithComment(comment));
            }
            CreateLesson newLesson = new CreateLesson()
                .WithThemaName(thema)
                .WithMentorId(mentorId)
                .WithStudentGroupId(studentGroup.Id)
                .WithLessonVisits(lessonVisits)
                .WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);
            var addLessonRequest = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", api.endpointsPath), Method.POST)
                .AddHeader("Authorization", api.GetToken(role))
                .AddJsonBody(jsonfile);
            var addLessonResponse = APIClient.client.Execute(addLessonRequest).StatusCode;
            Assert.AreEqual(addLessonResponse, expectedStatusCode, "Assert status code for adding lesson");
            api.log.Info($"Request is done with {addLessonResponse} StatusCode");
            var newrequest = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", api.endpointsPath), Method.GET)
                .AddHeader("Authorization", api.GetToken(role));
            var newresponse = APIClient.client.Execute(newrequest);
            int afterCount = JsonConvert.DeserializeObject<List<Lesson>>(newresponse.Content).Count;
            Assert.AreEqual(beforeCount + increment, afterCount, "Assert count of lessons");
            api.log.Info($"Expected and actual results is checked");
        }
    }
}
