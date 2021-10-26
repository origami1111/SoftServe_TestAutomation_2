using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
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
    public class PostAddsNewLesson : API_BaseTest
    {
        [TestCaseSource(typeof(TestCase), nameof(TestCase.ValidForAddsNewLesson))]
        public void LessonsPostAddsNewLesson(HttpStatusCode expectedStatusCode, Role role, string thema, int mentorId, string date, int mark, bool presense, string comment)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            var request = api.InitNewRequest("ApiStudentsGroup", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var response = APIClient.client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<StudentGroup>>(response.Content);
            var studentGroup = responseDetail.FirstOrDefault();
            
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            for (int i = 0; i < studentGroup.StudentIds.Count; i++)
            {
                lessonvisits
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
                .WithLessonVisits(lessonvisits)
                .WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var newrequest = api.InitNewRequest("Lessons", Method.POST, api.GetAuthenticatorFor(role)).AddJsonBody(jsonfile);
            var newresponse = APIClient.client.Execute(newrequest);
            api.log.Info($"Request is done with {newresponse.StatusCode} StatusCode");
            var actualCode = newresponse.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<Lesson>(newresponse.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(thema, resposneDetaile.ThemeName, "Thema name Assert");
                Assert.AreEqual(mentorId, resposneDetaile.MentorId, "Mentor Id Assert");
                Assert.AreEqual(studentGroup.Id, resposneDetaile.StudentGroupId, "Student group Id Assert");
                Assert.AreEqual(Convert.ToDateTime(date), resposneDetaile.LessonDate, "Lesson date Assert");
                for (int i = 0; i < resposneDetaile.LessonVisits.Count; i++)
                {
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].StudentId, lessonvisits[i].StudentId, "Student Id Assert");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].StudentMark, lessonvisits[i].StudentMark, "Student mark Assert");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].Presence, lessonvisits[i].Presence, "Presence Assert");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].Comment, lessonvisits[i].Comment, "Comment Assert");
                }
            });
            api.log.Info($"Expected and actual results is checked");
        }

        [TestCaseSource(typeof(TestCase), nameof(TestCase.ForbiddenForAddsNewLesson))]
        public void VerifyStatusCodeForbidden(HttpStatusCode expectedStatusCode, Role role, string thema, int mentorId, string date, int mark, bool presense, string comment)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            var request = api.InitNewRequest("ApiStudentsGroup", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var response = APIClient.client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<StudentGroup>>(response.Content);
            var studentGroup = responseDetail.FirstOrDefault();
            
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            for (int i = 0; i < studentGroup.StudentIds.Count; i++)
            {
                lessonvisits
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
                .WithLessonVisits(lessonvisits)
                .WithLessonDate(date);

            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var newrequest = api.InitNewRequest("Lessons", Method.POST, api.GetAuthenticatorFor(role)).AddJsonBody(jsonfile);

            var newresponse = APIClient.client.Execute(newrequest);
            api.log.Info($"Request is done with {newresponse.StatusCode} StatusCode");
            var actualStatusCode = newresponse.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            api.log.Info($"Expected and actual results is checked");
        }

        [TestCaseSource(typeof(TestCase), nameof(TestCase.UnauthorizedAddsNewLesson))]
        public void VerifyStatusCodeUnauthorized(HttpStatusCode expectedStatusCode, string thema, int mentorId, string date, int mark, bool presense, string comment)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            var request = api.InitNewRequest("ApiStudentsGroup", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var response = APIClient.client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<StudentGroup>>(response.Content);
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
            var newrequest = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", api.endpointsPath), Method.POST).AddJsonBody(jsonfile);

            var newresponse = APIClient.client.Execute(newrequest);
            api.log.Info($"Request is done with {newresponse.StatusCode} StatusCode");
            var actualStatusCode = newresponse.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            api.log.Info($"Expected and actual results is checked");
        }

        [TestCaseSource(typeof(TestCase), nameof(TestCase.BadRequestAddsNewLesson))]
        public void VerifyStatusCodeBadRequest(HttpStatusCode expectedStatusCode, Role role, string thema, int mentorId, int studentGroupId, string date, int mark, bool presense, string comment)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            CreateVisit lessonvisit1 = new CreateVisit()
                .WithStudentId(1)
                .WithStudentMark(mark)
                .WithPresence(presense)
                .WithComment(comment);
            lessonvisits.Add(lessonvisit1);
            CreateLesson newLesson = new CreateLesson()
                .WithThemaName(thema)
                .WithMentorId(mentorId)
                .WithStudentGroupId(studentGroupId)
                .WithLessonVisits(lessonvisits)
                .WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);
            var request = api.InitNewRequest("Lessons", Method.POST, api.GetAuthenticatorFor(role)).AddJsonBody(jsonfile);

            var response = APIClient.client.Execute(request);
            api.log.Info($"Request is done with {response.StatusCode} StatusCode");
            var actualStatusCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            api.log.Info($"Expected and actual results is checked");
        }
    }
}
