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
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    [AllureNUnit]
    public class GetCheckIfLessonWasDone : API_BaseTest
    {
        [TestCaseSource(typeof(TestCase),nameof(TestCase.ValidRole))]
        public void CheckIfLessonWasDone(HttpStatusCode expectedStatusCode, Role role)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(GetCheckIfLessonWasDone)}");
            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.GET)
                .AddHeader("Authorization", GetToken(Role.Admin));
            var response = client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<Lesson>>(response.Content);
            int lessonId = responseDetail
                .Where(l => l.LessonVisits.Any(s => s.Presence == true))
                .Select(l => l.Id)
                .First();

            var newrequest = new RestRequest($"lessons/{lessonId}/isdone", Method.GET)
                .AddHeader("Authorization", GetToken(role));           
            var newresponse = client.Execute(newrequest);
            var actualStatusCode = newresponse.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            Assert.AreEqual(true, Convert.ToBoolean(newresponse.Content));
            log.Info($"Expected and actual results is checked");
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        public void VerifyUnauthorizedStatusCode(HttpStatusCode expectedStatusCode)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(GetCheckIfLessonWasDone)}");
            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("Lessons", endpointsPath), Method.GET)
                .AddHeader("Authorization", GetToken(Role.Admin));
            var response = client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<Lesson>>(response.Content);
            int lessonId = responseDetail
                .Where(l => l.LessonVisits.Any(s => s.Presence == true))
                .Select(l => l.Id)
                .First();

            var newrequest = new RestRequest($"lessons/{lessonId}/isdone", Method.GET);
            var newresponse = client.Execute(newrequest);
            var actualStatusCode = newresponse.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            log.Info($"Expected and actual results is checked");
        }

        [TestCase(HttpStatusCode.NotFound, Int32.MaxValue)]
        public void VerifyNotFoundStatusCode(HttpStatusCode expectedStatusCode,int id)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(GetCheckIfLessonWasDone)}");
            var newrequest = new RestRequest($"lessons/{id}/isdone", Method.GET)
                .AddHeader("Authorization", GetToken(Role.Admin));       
            var newresponse = client.Execute(newrequest);
            var actualStatusCode = newresponse.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            log.Info($"Expected and actual results is checked");
        }
    }
}
