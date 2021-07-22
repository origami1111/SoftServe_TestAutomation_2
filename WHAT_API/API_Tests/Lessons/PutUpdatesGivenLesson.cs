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
    public class PutUpdatesGivenLesson : API_BaseTest
    {
        [TestCaseSource(typeof(TestCase), nameof(TestCase.ValidUpdatesGivenLesson))]
        public void UpdatesGivenLesson(HttpStatusCode expectedStatusCode, Role role)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PutUpdatesGivenLesson)}");
            var lessonRequest = InitNewRequest("Lessons", Method.GET, GetAuthenticatorFor(Role.Admin));
            var lessonResponse = client.Execute(lessonRequest);
            int lessonId = JsonConvert.DeserializeObject<List<Lesson>>(lessonResponse.Content).FirstOrDefault().Id;
            string themaName = Guid.NewGuid().ToString();
            DateTime date = DateTime.Now;
            UpdateLesson updatesGivenLesson = new UpdateLesson()
                .WithThemaName(themaName)
                .WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(updatesGivenLesson);

            var request = new RestRequest($"lessons/{lessonId}", Method.PUT)
                .AddHeader("Authorization", GetToken(role))
                .AddJsonBody(jsonfile);

            var response = client.Execute(request);
            var actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<Lesson>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(resposneDetaile.LessonDate, date, "Assert lesson date");
                Assert.AreEqual(resposneDetaile.ThemeName, themaName, "Assert thema name");
            });
            log.Info($"Expected and actual results is checked");
        }

        [TestCaseSource(typeof(TestCase), nameof(TestCase.ForbiddenUpdatesGivenLesson))]
        public void VerifyForbiddenStatusCode(HttpStatusCode expectedStatusCode, Role role)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PutUpdatesGivenLesson)}");
            var lessonRequest = InitNewRequest("Lessons", Method.GET, GetAuthenticatorFor(Role.Admin));
            var lessonResponse = client.Execute(lessonRequest);
            int lessonId = JsonConvert.DeserializeObject<List<Lesson>>(lessonResponse.Content).FirstOrDefault().Id;
            string themaName = Guid.NewGuid().ToString();
            DateTime date = DateTime.Now;
            UpdateLesson updatesGivenLesson = new UpdateLesson()
                .WithThemaName(themaName)
                .WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(updatesGivenLesson);

            var request = new RestRequest($"lessons/{lessonId}", Method.PUT)
                .AddHeader("Authorization", GetToken(role))
                .AddJsonBody(jsonfile);

            var response = client.Execute(request);
            var actuaStatuslCode = response.StatusCode;
            log.Info($"Request is done with {actuaStatuslCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actuaStatuslCode, "Status Code Assert");
            log.Info($"Expected and actual results is checked");
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        public void VerifyUnauthorizedStatusCode(HttpStatusCode expectedStatusCode)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PutUpdatesGivenLesson)}");
            var lessonRequest = InitNewRequest("Lessons", Method.GET, GetAuthenticatorFor(Role.Admin));
            var lessonResponse = client.Execute(lessonRequest);
            int lessonId = JsonConvert.DeserializeObject<List<Lesson>>(lessonResponse.Content).FirstOrDefault().Id;
            string themaName = Guid.NewGuid().ToString();
            DateTime date = DateTime.Now;
            UpdateLesson updatesGivenLesson = new UpdateLesson()
                .WithThemaName(themaName)
                .WithLessonDate(date);
            var jsonfile = JsonConvert.SerializeObject(updatesGivenLesson);

            var request = new RestRequest($"lessons/{lessonId}", Method.PUT)
                .AddJsonBody(jsonfile);

            var response = client.Execute(request);
            var actuaStatuslCode = response.StatusCode;
            log.Info($"Request is done with {actuaStatuslCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actuaStatuslCode, "Status Code Assert");
            log.Info($"Expected and actual results is checked");
        }
    }
}
