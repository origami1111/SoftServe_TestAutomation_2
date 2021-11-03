using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    [Category("ApiTest-Mentors")]
    class GET_GetMentorCourses_Unauthorised : API_BaseTest
    {
        WhatAccount mentor;
        CourseDto course;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            mentor = api.RegistrationUser(newUser);
            mentor = api.AssignRole(mentor, Role.Mentor);
            course = api.CreateCourse(new CreateCourseDto());
            api.AssignCourseToMentor(course, mentor);
        }

        [Test]
        public void VerifyGetMentorCourses_Unauthorised()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorCourses_Unauthorised)}");            

            var endpoint = "ApiMentorsIdCourses";
            var request = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.GET);
            request.AddUrlSegment("id", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
            api.DisableCourse(course);
        }
    }
}
