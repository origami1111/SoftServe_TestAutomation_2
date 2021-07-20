using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
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
        [Test]
        [TestCase(HttpStatusCode.OK, "Some theme", 2, 22, "2015-07-20T18:30:25", 1)]
        public void GetsListOfAllLessons(HttpStatusCode expectedStatusCode, string thema, int mentorId, int studentGroupId, string date, int increment)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(GetListOfAllLessons)}");
            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.GET)
                .AddHeader("Authorization", GetToken(Role.Admin));
            var response = client.Execute(request);
            var actualCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualCode, "Assert status code");
            int beforeCount = JsonConvert.DeserializeObject<List<ResponseAddsNewLesson>>(response.Content).Count;
            log.Info($"Request is done with {response.StatusCode} StatusCode");

            List<LessonVisit> lessonvisits = new List<LessonVisit>();
            LessonVisit lessonvisit1 = new LessonVisit().WithStudentId(1).WithStudentMark(5).WithPresence(true).WithComment("");
            lessonvisits.Add(lessonvisit1);
            LessonVisit lessonvisit2 = new LessonVisit().WithStudentId(2).WithStudentMark(null).WithPresence(false).WithComment("");
            lessonvisits.Add(lessonvisit2);
            LessonVisit lessonvisit3 = new LessonVisit().WithStudentId(3).WithStudentMark(null).WithPresence(true).WithComment("");
            lessonvisits.Add(lessonvisit3);
            AddsNewLesson newLesson = new AddsNewLesson().WithThemaName(thema).WithMentorId(mentorId).WithStudentGroupId(studentGroupId)
                .WithLessonVisits(lessonvisits).WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);
            var addLessonRequest = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.POST)
                .AddHeader("Authorization", GetToken(Role.Admin))
                .AddJsonBody(jsonfile);
            var addLessonResponse = client.Execute(addLessonRequest).StatusCode;
            Assert.AreEqual(addLessonResponse, expectedStatusCode, "Assert status code for adding lesson");

            var newrequest = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.GET)
                .AddHeader("Authorization", GetToken(Role.Admin));
            var newresponse = client.Execute(newrequest);
            int afterCount = JsonConvert.DeserializeObject<List<ResponseAddsNewLesson>>(newresponse.Content).Count;
            Assert.AreEqual(beforeCount + increment, afterCount, "Assert count of lessons");
        }
    }
}
