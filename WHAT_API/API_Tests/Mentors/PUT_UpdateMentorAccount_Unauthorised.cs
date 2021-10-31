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
    class PUT_UpdateMentorAccount_Unauthorised : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount student;
        CourseDto course;
        StudentGroupDto group;

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
        }

        [Test]
        public void VerifyUpdateMentorAccount_Unauthorised()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(PUT_UpdateMentorAccount_Unauthorised)}");
            var newMentorInfo = new UpdateMentorDto()
            {
                FirstName = StringGenerator.GenerateStringOfLetters(30),
                LastName = StringGenerator.GenerateStringOfLetters(30),
                Email = StringGenerator.GenerateEmail(),
                CourseIds = new List<int> { course.Id },
                StudentGroupIds = new List<int> { group.Id }
            };

            var endpoint = "ApiMentorId";
            var request = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.PUT);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            request.AddJsonBody(newMentorInfo);
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
            api.DisableAccount(student, Role.Student);
            api.DisableCourse(course);
        }
    }
}
