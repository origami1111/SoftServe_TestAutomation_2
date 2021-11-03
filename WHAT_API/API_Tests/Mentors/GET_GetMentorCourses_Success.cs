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
    [TestFixture(Role.Admin)]
    [TestFixture(Role.Secretary)]
    [TestFixture(Role.Mentor)]
    [Category("ApiTest-Mentors")]
    [AllureNUnit]
    class GET_GetMentorCourses_Success : API_BaseTest
    {
        WhatAccount mentor;
        CourseDto course;
        WhatAccount infoGetter;
        Credentials infoGetterCredentials;

        Role role;

        public GET_GetMentorCourses_Success(Role role) : base()
        {
            this.role = role;
        }

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

            if (role == Role.Admin)
            {
                infoGetterCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            if (role == Role.Mentor)
            {
                infoGetter = mentor;
                infoGetterCredentials = new Credentials { Email = newUser.Email, Password = newUser.Password, Role = role };
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            infoGetter = api.RegistrationUser(userInfoGetter);
            infoGetter = api.AssignRole(infoGetter, role);
            infoGetterCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyGetMentorCourses_Success()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorCourses_Success)}");            

            var endpoint = "ApiMentorsIdCourses";
            var authenticator = api.GetAuthenticatorFor(infoGetterCredentials);
            var request = api.InitNewRequest(endpoint, Method.GET, authenticator);
            request.AddUrlSegment("id", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string contentJson = response.Content;
            var courses = JsonConvert.DeserializeObject<List<CourseDto>>(contentJson);
            var foundCourse = courses.Find(m => m.Id == course.Id);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(course.Id, foundCourse.Id);
                Assert.AreEqual(course.Name, foundCourse.Name);
            });
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(infoGetter, role);
            }
            api.DisableAccount(mentor, Role.Mentor);
            api.DisableCourse(course);
        }
    }
}
