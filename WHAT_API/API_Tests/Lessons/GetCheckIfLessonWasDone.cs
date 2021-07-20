using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WHAT_API.Entities;
using WHAT_API.Entities.Lessons;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    [AllureNUnit]
    public class GetCheckIfLessonWasDone : API_BaseTest
    {
        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin)]
        //[TestCase(HttpStatusCode.OK, Role.Mentor)]
        //[TestCase(HttpStatusCode.OK, Role.Secretary)]
        
        public void CheckIfLessonWasDone(HttpStatusCode expectedStatusCode, Role role)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.GET)
                .AddHeader("Authorization", GetToken(role));
            var response = client.Execute(request);
            var actualCode = response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualCode, "Assert status code");

            var responseDetail = JsonConvert.DeserializeObject<List<Lesson>>(response.Content);
            foreach (var lesson in responseDetail)
            {
                if (lesson.LessonVisits.Count!=0)
                {
                    foreach (var student in lesson.LessonVisits)
                    {
                        if (student.Presence == true)
                        {
                            var newrequest = new RestRequest($"lessons/{lesson.Id}/isdone", Method.GET)
                                .AddHeader("Authorization", GetToken(role));
                            var newresponse = client.Execute(newrequest);
                            var actualStatusCode = newresponse.StatusCode;
                            log.Info($"Request is done with {actualStatusCode} StatusCode");
                            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
                            Assert.AreEqual(true, Convert.ToBoolean(newresponse.Content));
                            break;
                        }
                    }
                    break;
                }
            }
            log.Info($"Expected and actual results is checked");
        }
    }
}
