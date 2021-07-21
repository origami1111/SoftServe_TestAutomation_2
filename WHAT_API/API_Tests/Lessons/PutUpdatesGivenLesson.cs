using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_API.Entities;
using WHAT_API.Entities.Lessons;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    public class PutUpdatesGivenLesson : API_BaseTest
    {
        [Test]
        [TestCase(HttpStatusCode.OK, 1, true, "testComment","themaName", "2015-08-20T18:30:25", 5)]
        public void UpdatesGivenLesson(HttpStatusCode expectedStatusCode, int id, bool presense, string comment,string themaName,string date,int mark)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            List<CreateVisit> lessonvisits = new List<CreateVisit>();
            CreateVisit lessonvisit1 = new CreateVisit().WithStudentId(1).WithStudentMark(mark).WithPresence(presense).WithComment(comment);
            lessonvisits.Add(lessonvisit1);
            CreateVisit lessonvisit2 = new CreateVisit().WithStudentId(4).WithStudentMark(mark).WithPresence(presense).WithComment(comment);
            lessonvisits.Add(lessonvisit2);
            CreateVisit lessonvisit3 = new CreateVisit().WithStudentId(3).WithStudentMark(mark).WithPresence(presense).WithComment(comment);
            lessonvisits.Add(lessonvisit3);
            UpdateLesson updatesGivenLesson = new UpdateLesson().WithThemaName(themaName).WithLessonDate(date).WithLessonVisits(lessonvisits);
            var jsonfile = JsonConvert.SerializeObject(updatesGivenLesson);

            
            var request = new RestRequest($"lessons/{id}", Method.PUT)
                .AddHeader("Authorization", GetToken(Role.Admin))
                .AddJsonBody(jsonfile);

            var response = client.Execute(request);
            var actualCode = response.StatusCode;
            log.Info($"Request is done with {actualCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<Lesson>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(resposneDetaile.LessonDate, Convert.ToDateTime(date), "Assert lesson date");
                Assert.AreEqual(resposneDetaile.ThemeName, themaName, "Assert thema name");

                for (int i = 0; i < resposneDetaile.LessonVisits.Count; i++)
                {
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].StudentMark, mark, "Assert mark");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].Presence, presense, "Assert presense");
                    Assert.AreEqual(resposneDetaile.LessonVisits[i].Comment, comment, "Assert comment");
                }
            });
            log.Info($"Expected and actual results is checked");
        }
    }
}
