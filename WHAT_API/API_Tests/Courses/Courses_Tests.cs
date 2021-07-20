using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class Courses_Tests : API_BaseTest
    {
        private static string GenerateNameOf<T>() =>
            $"Test {typeof(T).Name} {Guid.NewGuid().ToString("N")}";

        private static string[] invalidCourseName = new string[]
        {
            "a",
            "Course name with more than 50 characters is too long",
            " Space before course name",
            "More than one space   between words",
            "Space after course name ",
            "Course name with special symbols?",
            "Not only Latin letters Кириллица",
            String.Empty
        };

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void AddNewCourse_ValidCourseName_VerifyContent(Role role)
        {
            var expected = new CreateOrUpdateCourse(GenerateNameOf<Course>());

            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(expected);
            var actual = Execute<Course>(addCourseRequest);
            Assert.NotNull(actual);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.True(actual.IsActive);
            });
        }

        [Test]
        public void AddNewCourse_InvalidCourseName_VerifyStatusCode(
            [Values(Role.Admin, Role.Secretary)] Role role,
            [ValueSource(nameof(invalidCourseName))] string invalidCourseName)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(invalidCourseName));
            var actual = client.Execute(addCourseRequest);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, actual.StatusCode);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void AddNewCourse_SameCourseName(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            client.Execute(addCourseRequest);
            var actual = client.Execute(addCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Conflict, actual.StatusCode);
                StringAssert.Contains("Course already exists", actual.Content);
            });
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Unassigned)]
        public void AddNewCourse_ForbiddenStatusCode(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = client.Execute(addCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode);
        }

        [Test]
        public void AddNewCourse_UnauthorizedStatusCode()
        {
            var resource = ReaderUrlsJSON.ByName("Add new course", endpointsPath);
            var addCourseRequest = new RestRequest(resource, Method.POST);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = client.Execute(addCourseRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Test]
        public void GetCourses_ActiveNotActive(
            [Values(Role.Student, Role.Mentor, Role.Secretary, Role.Admin)] Role role,
            [Values] bool isActive)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest getCoursesRequest = InitNewRequest("Get courses", Method.GET, authenticator);
            getCoursesRequest.AddQueryParameter("isActive", isActive.ToString());
            var actualCourses = Execute<List<Course>>(getCoursesRequest);

            Assert.NotNull(actualCourses);
            Assert.Multiple(() =>
            {
                CollectionAssert.IsNotEmpty(actualCourses);
                Assert.True(actualCourses.All(course => course.IsActive == isActive));
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void GetCourses_All(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest getCoursesRequest = InitNewRequest("Get courses", Method.GET, authenticator);
            var actualCourses = Execute<List<Course>>(getCoursesRequest);
            Assert.NotNull(actualCourses);
            
            getCoursesRequest.AddQueryParameter("isActive", "true");
            var activeCourses = Execute<List<Course>>(getCoursesRequest);
            getCoursesRequest.AddOrUpdateParameter("isActive", "false", ParameterType.QueryString);
            var notActiveCourses = Execute<List<Course>>(getCoursesRequest);
            var expectedCourses = activeCourses.Concat(notActiveCourses).ToList();

            Assert.Multiple(() =>
            {
                CollectionAssert.IsNotEmpty(actualCourses);
                CollectionAssert.AreEquivalent(expectedCourses, actualCourses);
            });
        }

        [Test]
        public void GetCourses_ActiveNotActive_ForbiddenStatusCode([Values] bool? isActive)
        {
            var authenticator = GetAuthenticatorFor(Role.Unassigned);
            RestRequest getCoursesRequest = InitNewRequest("Get courses", Method.GET, authenticator);
            getCoursesRequest.AddQueryParameter("isActive", isActive.ToString());
            var actual = client.Execute(getCoursesRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode);
        }

        [Test]
        public void GetCourses_ActiveNotActive_UnauthorizedStatusCode([Values] bool? isActive)
        {
            var resource = ReaderUrlsJSON.ByName("Get courses", endpointsPath);
            var getCoursesRequest = new RestRequest(resource, Method.GET);
            getCoursesRequest.AddQueryParameter("isActive", isActive.ToString());
            var actual = client.Execute(getCoursesRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_ValidCourseName(Role role)
        {
            var expected = new CreateOrUpdateCourse(GenerateNameOf<Course>());

            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest updateCourseRequest = InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(expected);
            var actual = Execute<Course>(updateCourseRequest);
            Assert.NotNull(actual);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.True(actual.IsActive);
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_SameCourseName(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            var courseName = new CreateOrUpdateCourse(GenerateNameOf<Course>());
            addCourseRequest.AddJsonBody(courseName);
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest updateCourseRequest = InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(courseName);
            var actual = client.Execute(updateCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Conflict, actual.StatusCode);
                StringAssert.Contains("Course already exist", actual.Content);
            });
        }

        [Test]
        public void UpdateCourse_InvalidCourseName_VerifyStatusCode(
            [Values(Role.Admin, Role.Secretary)] Role role,
            [ValueSource(nameof(invalidCourseName))] string invalidCourseName)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest updateCourseRequest = InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(invalidCourseName));
            var actual = client.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, actual.StatusCode, "Update course with bad request - status code test");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_IncorrectRequestFormat_VerifyStatusCode(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest updateCourseRequest = InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(GenerateNameOf<Course>());
            var actual = client.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode, "Update course with bad request - status code test");
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Unassigned)]
        public void UpdateCourse_ForbiddenStatusCode(Role role)
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            authenticator = GetAuthenticatorFor(role);
            RestRequest updateCourseRequest = InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = client.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode);
        }

        [Test]
        public void UpdateCourse_UnauthorizedStatusCode()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            var resource = ReaderUrlsJSON.ByName("Update course", endpointsPath);
            var updateCourseRequest = new RestRequest(resource, Method.PUT);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = client.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void DisableCourse_Success(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);
            
            RestRequest disableCourseRequest = InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = Execute<bool>(disableCourseRequest);
            
            Assert.IsTrue(actual);
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Unassigned)]
        public void DisableCourse_ForbiddenStatusCode(Role role)
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            authenticator = GetAuthenticatorFor(role);
            RestRequest disableCourseRequest = InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = client.Execute(disableCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode);
        }

        [Test]
        public void DisableCourse_UnauthorizedStatusCode()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            var resource = ReaderUrlsJSON.ByName("Disable course", endpointsPath);
            var disableCourseRequest = new RestRequest(resource, Method.DELETE);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = client.Execute(disableCourseRequest);
           
            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void DisableCourse_WithActiveStudentGroup(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest addStudentGroupRequest = InitNewRequest("Add new student group", Method.POST, authenticator);
            var studentGroup = new CreateStudentGroup { CourseId = originCourse.Id };
            addStudentGroupRequest.AddJsonBody(studentGroup);
            client.Execute(addStudentGroupRequest);

            RestRequest disableCourseRequest = InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = client.Execute(disableCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
                StringAssert.Contains("Course has active student group", actual.Content);
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void EnableCourse_Success(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest disableCourseRequest = InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            Execute<bool>(disableCourseRequest);

            RestRequest enableCourseRequest = InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = Execute<bool>(enableCourseRequest);

            Assert.IsTrue(actual);
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Unassigned)]
        public void EnableCourse_ForbiddenStatusCode(Role role)
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);
            
            RestRequest disableCourseRequest = InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            Execute<bool>(disableCourseRequest);

            authenticator = GetAuthenticatorFor(role);
            RestRequest enableCourseRequest = InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = client.Execute(enableCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode);
        }

        [Test]
        public void EnableCourse_UnauthorizedStatusCode()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest disableCourseRequest = InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            Execute<bool>(disableCourseRequest);

            var resource = ReaderUrlsJSON.ByName("Enable course", endpointsPath);
            var enableCourseRequest = new RestRequest(resource, Method.PATCH);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = client.Execute(enableCourseRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void EnableCourse_CourseAlreadyActive(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = Execute<Course>(addCourseRequest);

            RestRequest enableCourseRequest = InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = client.Execute(enableCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Conflict, actual.StatusCode);
                StringAssert.Contains("Course is already active", actual.Content);
            });
        }
    }
}