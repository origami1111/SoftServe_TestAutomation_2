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
    [AllureNUnit]
    class PUT_UpdateMentorAccount_Success : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount student;
        CourseDto course;
        StudentGroupDto group;
        WhatAccount accountUpdater;
        Credentials accountUpdaterCredentials;

        Role role;

        public PUT_UpdateMentorAccount_Success(Role role) : base()
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

            var newStudent = new GenerateUser();
            student = api.RegistrationUser(newStudent);
            student = api.AssignRole(student, Role.Student);

            course = api.CreateCourse(new CreateCourseDto());
            var newGroup = new CreateStudentGroupDto
            {
                CourseId = course.Id,
                StudentIds = new List<int> { student.Id },
                MentorIds = new List<int> { mentor.Id }
            };
            group = api.CreateStudentGroup(newGroup);

            if (role == Role.Admin)
            {
                accountUpdaterCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            accountUpdater = api.RegistrationUser(userInfoGetter);
            accountUpdater = api.AssignRole(accountUpdater, role);
            accountUpdaterCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyUpdateMentorAccount_Success()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(PUT_UpdateMentorAccount_Success)}");
            var newMentorInfo = new UpdateMentorDto()
            {
                FirstName = StringGenerator.GenerateStringOfLetters(30),
                LastName = StringGenerator.GenerateStringOfLetters(30),
                Email = StringGenerator.GenerateEmail(),
                CourseIds = new List<int> { course.Id },
                StudentGroupIds = new List<int> { group.Id }
            };

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountUpdaterCredentials);
            var request = api.InitNewRequest(endpoint, Method.PUT, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            request.AddJsonBody(newMentorInfo);
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string contentJson = response.Content;
            var updatedMentor = JsonConvert.DeserializeObject<WhatAccount>(contentJson);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(mentor.Id, updatedMentor.Id);
                Assert.AreEqual(newMentorInfo.FirstName, updatedMentor.FirstName);
                Assert.AreEqual(newMentorInfo.LastName, updatedMentor.LastName);
                Assert.AreEqual(newMentorInfo.Email, updatedMentor.Email);
            });
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(accountUpdater, role);
            }
            api.DisableAccount(mentor, Role.Mentor);
            api.DisableAccount(student, Role.Student);
            api.DisableCourse(course);
        }
    }
}
