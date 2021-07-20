﻿using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WHAT_API.Entities.Lessons;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    [AllureNUnit]
    public class GetCheckIfLessonWasDone : API_BaseTest
    {
        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin, 1)]
        [TestCase(HttpStatusCode.OK, Role.Mentor, 1)]
        [TestCase(HttpStatusCode.OK, Role.Secretary, 1)]
        [TestCase(HttpStatusCode.OK, Role.Student, 1)]
        [TestCase(HttpStatusCode.OK, Role.Unassigned, 1)]
        public void CheckIfLessonWasDone(HttpStatusCode expectedStatusCode, Role role, int id)
        {
            log = LogManager.GetLogger($"Lessons/{nameof(PostAddsNewLesson)}");
            var request = new RestRequest($"lessons/{id}/isdone", Method.GET)
                .AddHeader("Authorization", GetToken(role));

            var response = client.Execute(request);
            var actualCode = response.StatusCode;
            log.Info($"Request is done with {actualCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualCode, "Status Code Assert");
            
            Assert.AreEqual(true, Convert.ToBoolean(response.Content));
            log.Info($"Expected and actual results is checked");
        }
    }
}
