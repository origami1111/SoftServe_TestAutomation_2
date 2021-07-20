﻿using Newtonsoft.Json;
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
        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment")]
        public void LessonsPostAddsNewLesson(HttpStatusCode expectedStatusCode, Role role, string thema, int mentorId, string date, int mark, bool presense, string comment)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
           
            var request = InitNewRequest("ApiStudentsGroup", Method.GET, GetAuthenticatorFor(Role.Admin));
            var response = client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<StudentGroup>>(response.Content);

            var studentGroup = responseDetail.FirstOrDefault();
            int studentGroupId = studentGroup.ID;
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            for (int i = 0; i < studentGroup.StudentIds.Count; i++)
            {
                lessonvisits.Add(new CreateVisit().WithStudentId(studentGroup.StudentIds[i]).WithStudentMark(mark).WithPresence(presense).WithComment(comment));
            }
            CreateLesson newLesson = new CreateLesson().WithThemaName(thema).WithMentorId(mentorId).WithStudentGroupId(studentGroupId)
               .WithLessonVisits(lessonvisits).WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var newrequest = InitNewRequest("Lessons", Method.POST, GetAuthenticatorFor(role)).AddJsonBody(jsonfile);

            var newresponse = client.Execute(newrequest);
            log.Info($"Request is done with {newresponse.StatusCode} StatusCode");
            var actualCode = newresponse.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualCode, "Status Code Assert");

            
            var resposneDetaile = JsonConvert.DeserializeObject<Lesson>(newresponse.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(thema, resposneDetaile.ThemeName, "Thema name Assert");
                Assert.AreEqual(mentorId, resposneDetaile.MentorId, "Mentor Id Assert");
                Assert.AreEqual(studentGroupId, resposneDetaile.StudentGroupId, "Student group Id Assert");
                Assert.AreEqual(Convert.ToDateTime(date), resposneDetaile.LessonDate, "Lesson date Assert");
                for (int i = 0; i < resposneDetaile.LessonVisits.Count; i++)
                {
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].StudentId, lessonvisits[i].StudentId, "Student Id Assert");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].StudentMark, lessonvisits[i].StudentMark, "Student mark Assert");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].Presence, lessonvisits[i].Presence, "Presence Assert");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].Comment, lessonvisits[i].Comment, "Comment Assert");
                }
            });
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Forbidden, Role.Student, "Some theme", 2, 22, "2015-07-20T18:30:25", "myComment")]
        [TestCase(HttpStatusCode.Forbidden, Role.Secretary, "Some theme", 2, 22, "2015-07-20T18:30:25", "myComment")]
        [TestCase(HttpStatusCode.Forbidden, Role.Unassigned, "Some theme", 2, 22, "2015-07-20T18:30:25", "myComment")]
        public void VerifyStatusCodeForbidden(HttpStatusCode expectedStatusCode, Role role, string thema, int mentorId, int studentGroupId, string date, string comment)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            CreateVisit lessonvisit1 = new CreateVisit().WithStudentId(1).WithStudentMark(null).WithPresence(true).WithComment(comment);
            lessonvisits.Add(lessonvisit1);
            CreateVisit lessonvisit2 = new CreateVisit().WithStudentId(2).WithStudentMark(null).WithPresence(false).WithComment(comment);
            lessonvisits.Add(lessonvisit2);
            CreateVisit lessonvisit3 = new CreateVisit().WithStudentId(3).WithStudentMark(null).WithPresence(true).WithComment(comment);
            lessonvisits.Add(lessonvisit3);
            CreateLesson newLesson = new CreateLesson().WithThemaName(thema).WithMentorId(mentorId).WithStudentGroupId(studentGroupId)
                .WithLessonVisits(lessonvisits).WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var request = InitNewRequest("Lessons", Method.POST, GetAuthenticatorFor(role)).AddJsonBody(jsonfile);

            var response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            var actualStatusCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
        }

        [Test]
        [TestCase(HttpStatusCode.Unauthorized, "Some theme", 2, 22, "2015-07-20T18:30:25", "myComment")]
        public void VerifyStatusCodeUnauthorized(HttpStatusCode expectedStatusCode, string thema, int mentorId, int studentGroupId, string date, string comment)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            CreateVisit lessonvisit1 = new CreateVisit().WithStudentId(1).WithStudentMark(null).WithPresence(true).WithComment(comment);
            lessonvisits.Add(lessonvisit1);
            CreateVisit lessonvisit2 = new CreateVisit().WithStudentId(2).WithStudentMark(null).WithPresence(false).WithComment(comment);
            lessonvisits.Add(lessonvisit2);
            CreateVisit lessonvisit3 = new CreateVisit().WithStudentId(3).WithStudentMark(null).WithPresence(true).WithComment(comment);
            lessonvisits.Add(lessonvisit3);
            CreateLesson newLesson = new CreateLesson().WithThemaName(thema).WithMentorId(mentorId).WithStudentGroupId(studentGroupId)
                .WithLessonVisits(lessonvisits).WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.POST).AddJsonBody(jsonfile);

            var response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            var actualStatusCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
        }

        [Test]
        [TestCase(HttpStatusCode.BadRequest, Role.Admin, "Some theme", 2, 22, "2015-07-20T18:30:25", "myComment")]
        public void VerifyStatusCodeBadRequest(HttpStatusCode expectedStatusCode, Role role, string thema, int mentorId, int studentGroupId, string date, string comment)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            CreateVisit lessonvisit1 = new CreateVisit().WithStudentId(1).WithStudentMark(null).WithPresence(true).WithComment(comment);
            lessonvisits.Add(lessonvisit1);
            CreateLesson newLesson = new CreateLesson().WithThemaName(thema).WithMentorId(mentorId).WithStudentGroupId(studentGroupId)
                .WithLessonVisits(lessonvisits).WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var request = InitNewRequest("Lessons", Method.POST, GetAuthenticatorFor(role)).AddJsonBody(jsonfile);

            var response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            var actualStatusCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
        }
    }
}
