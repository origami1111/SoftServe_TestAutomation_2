using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_API.Entities;
using WHAT_API.Entities.Lessons;
using WHAT_Utilities;

using static WHAT_API.Entities.Lessons.AddsNewLesson;


namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    public class PostAddsNewLesson : API_BaseTest
    {
        [Test]
        [TestCase("Some theme", 2, 22, "2015-07-20T18:30:25")]
        public void LessonsPostAddsNewLesson(string thema, int mentorId, int studentGroupId, string date)
        {
            List<Lessonvisit> lessonvisits = new List<Lessonvisit>();
            Lessonvisit lessonvisit1 = new Lessonvisit().WithStudentId(1).WithStudentMark(5).WithPresence(true).WithComment("");
            lessonvisits.Add(lessonvisit1);
            Lessonvisit lessonvisit2 = new Lessonvisit().WithStudentId(2).WithStudentMark(null).WithPresence(false).WithComment("");
            lessonvisits.Add(lessonvisit2);
            Lessonvisit lessonvisit3 = new Lessonvisit().WithStudentId(3).WithStudentMark(null).WithPresence(true).WithComment("");
            lessonvisits.Add(lessonvisit3);
            AddsNewLesson newLesson = new AddsNewLesson().WithThemaName(thema).WithMentorId(mentorId).WithStudentGroupId(studentGroupId)
                .WithLessonVisits(lessonvisits).WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(newLesson);

            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.POST)
                .AddHeader("Authorization", GetToken(Role.Admin))
                .AddJsonBody(jsonfile);

            var response = client.Execute(request);
            var expectedCode = HttpStatusCode.OK;
            var actualCode = response.StatusCode;
            Assert.AreEqual(expectedCode, actualCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<ResponseAddsNewLesson>(response.Content);
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
        }
    }
}
