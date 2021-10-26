using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class CoursesTests : API_BaseTest
    {
        private static string GenerateNameOf<T>() =>
            $"Test {typeof(T).Name} {Guid.NewGuid():N}";

        private static readonly string[] invalidCourseName = new string[]
        {
            "a",
            "Course name with more than 50 characters is too long",
            " Space before course name",
            "More than one space   between words",
            "Space after course name ",
            "Course name with special symbols?",
            "Not only Latin letters Кириллица",
            ""
        };

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void AddNewCourse_ValidCourseName_VerifyContent(Role role)
        {
            var expected = new CreateOrUpdateCourse(GenerateNameOf<Course>());

            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(expected);
            var actual = api.Execute<Course>(addCourseRequest);
            
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode, "Http Status code");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Name, actual.Data.Name, "Course name");
                Assert.True(actual.Data.IsActive, "Is new course active");
            });
        }

        [Ignore("Data validation hasn't implemented")]
        [Test]
        public void AddNewCourse_InvalidCourseName_IsStatusCodeUnprocessableEntity(
            [Values(Role.Admin, Role.Secretary)] Role role,
            [ValueSource(nameof(invalidCourseName))] string invalidCourseName)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(invalidCourseName));
            var actual = api.Execute<object>(addCourseRequest);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void AddNewCourse_SameCourseName_IsStatusCodeConflict(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            api.Execute(addCourseRequest);
            var actual = api.Execute<object>(addCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Conflict, actual.StatusCode, "Http Status Code");
                StringAssert.Contains("Course already exists", actual.Content, "Error message");
            });
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void AddNewCourse_ForbiddenRole_IsStatusCodeForbidden(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = api.Execute(addCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode, "Http Status Code");
        }

        [Test]
        public void AddNewCourse_UnauthorizedUser_IsStatusCodeUnauthorized()
        {
            var resource = ReaderUrlsJSON.ByName("Add new course", api.endpointsPath);
            var addCourseRequest = new RestRequest(resource, Method.POST);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = api.Execute(addCourseRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode, "Http Status Code");
        }

        [Test]
        public void GetCourses_ActiveNotActive_IsSuccess(
            [Values(Role.Admin, Role.Student, Role.Mentor, Role.Secretary)] Role role,
            [Values] bool isActive)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest getCoursesRequest = api.InitNewRequest("Get courses", Method.GET, authenticator);
            getCoursesRequest.AddQueryParameter("isActive", isActive.ToString());
            var actualCourses = api.Execute<List<Course>>(getCoursesRequest);

            Assert.AreEqual(HttpStatusCode.OK, actualCourses.StatusCode, "Http Status code");
            Assert.Multiple(() =>
            {
                CollectionAssert.IsNotEmpty(actualCourses.Data, "List of courses");
                Assert.True(actualCourses.Data.All(course => course.IsActive == isActive),
                    $"Are all courses {isActive}");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void GetCourses_All_IsAllCoursesEqualActiveAndNotActive(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest getCoursesRequest = api.InitNewRequest("Get courses", Method.GET, authenticator);
            var actualCourses = api.Execute<List<Course>>(getCoursesRequest);
            Assert.AreEqual(HttpStatusCode.OK, actualCourses.StatusCode, "Http Status code");

            getCoursesRequest.AddQueryParameter("isActive", "true");
            var activeCourses = api.Execute<List<Course>>(getCoursesRequest).Data;
            getCoursesRequest.AddOrUpdateParameter("isActive", "false", ParameterType.QueryString);
            var notActiveCourses = api.Execute<List<Course>>(getCoursesRequest).Data;
            var expectedCourses = activeCourses.Concat(notActiveCourses).ToList();

            Assert.Multiple(() =>
            {
                CollectionAssert.IsNotEmpty(actualCourses.Data, "List of courses");
                CollectionAssert.AreEquivalent(expectedCourses, actualCourses.Data,
                    "Active and not active courses are equivalent all courses");
            });
        }

        [Test]
        public void GetCourses_UnauthorizedUser_UnauthorizedStatusCode([Values] bool? isActive)
        {
            var resource = ReaderUrlsJSON.ByName("Get courses", api.endpointsPath);
            var getCoursesRequest = new RestRequest(resource, Method.GET);
            getCoursesRequest.AddQueryParameter("isActive", isActive.ToString());
            var actual = api.Execute(getCoursesRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_ValidCourseName_IsNameUpdated(Role role)
        {
            var expected = new CreateOrUpdateCourse(GenerateNameOf<Course>());

            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest updateCourseRequest = api.InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(expected);
            var actual = api.Execute<Course>(updateCourseRequest);
            
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode, "Http Status code");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Name, actual.Data.Name, "Course name");
                Assert.True(actual.Data.IsActive, "Is updated course active");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_SameCourseName_IsStatusCodeConflict(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            var courseName = new CreateOrUpdateCourse(GenerateNameOf<Course>());
            addCourseRequest.AddJsonBody(courseName);
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest updateCourseRequest = api.InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(courseName);
            var actual = api.Execute(updateCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Conflict, actual.StatusCode, "Http Status Code");
                StringAssert.Contains("Course already exist", actual.Content, "Error message");
            });
        }

        [Ignore("Data validation hasn't implemented")]
        [Test]
        public void UpdateCourse_InvalidCourseName_IsStatusCodeUnprocessableEntity(
            [Values(Role.Admin, Role.Secretary)] Role role,
            [ValueSource(nameof(invalidCourseName))] string invalidCourseName)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest updateCourseRequest = api.InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(invalidCourseName));
            var actual = api.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_IncorrectCourseId_IsStatusCodeNotFound(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest updateCourseRequest = api.InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", long.MaxValue);
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = api.Execute(updateCourseRequest);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, actual.StatusCode, "Http Status Code");
                StringAssert.Contains("Not Found", actual.Content, "Error message");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateCourse_IncorrectRequestFormat_IsStatusCodeBadRequest(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest updateCourseRequest = api.InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(GenerateNameOf<Course>());
            var actual = api.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void UpdateCourse_ForbiddenRole_IsStatusCodeForbidden(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            authenticator = api.GetAuthenticatorFor(role);
            RestRequest updateCourseRequest = api.InitNewRequest("Update course", Method.PUT, authenticator);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = api.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode, "Http Status Code");
        }

        [Test]
        public void UpdateCourse_UnauthorizedUser_IsStatusCodeUnauthorized()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            var resource = ReaderUrlsJSON.ByName("Update course", api.endpointsPath);
            var updateCourseRequest = new RestRequest(resource, Method.PUT);
            updateCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            updateCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var actual = api.Execute(updateCourseRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void DisableCourse_WithoutActiveStudentGroup_IsTrue(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;
            
            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute<bool>(disableCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode, "Http Status Code");
                Assert.IsTrue(actual.Data, "Is course without active student group disabled");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void DisableCourse_IncorrectCourseId_IsStatusCodeNotFound(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", long.MaxValue);
            var actual = api.Execute(disableCourseRequest);

            Assert.AreEqual(HttpStatusCode.NotFound, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void DisableCourse_ForbiddenUser_IsStatusCodeForbidden(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            authenticator = api.GetAuthenticatorFor(role);
            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute(disableCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode, "Http Status Code");
        }

        [Test]
        public void DisableCourse_UnauthorizedUser_IsStatusCodeUnauthorized()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            var resource = ReaderUrlsJSON.ByName("Disable course", api.endpointsPath);
            var disableCourseRequest = new RestRequest(resource, Method.DELETE);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute(disableCourseRequest);
           
            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void DisableCourse_WithActiveStudentGroup_IsStatusCodeBadRequest(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest addStudentGroupRequest = api.InitNewRequest("Add new student group", Method.POST, authenticator);
            var studentGroup = new CreateStudentGroup { CourseId = originCourse.Id };
            addStudentGroupRequest.AddJsonBody(studentGroup);
            api.Execute(addStudentGroupRequest);

            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute(disableCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode, "Http Status Code");
                StringAssert.Contains("Course has active student group", actual.Content, "Error message");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void EnableCourse_IsSuccess(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            api.Execute<bool>(disableCourseRequest);

            RestRequest enableCourseRequest = api.InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute<bool>(enableCourseRequest);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode, "Http Status Code");
                Assert.IsTrue(actual.Data, "Is course enabled");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void EnableCourse_IncorrectCourseId_IsStatusCodeNotFound(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest enableCourseRequest = api.InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", long.MaxValue);
            var actual = api.Execute(enableCourseRequest);

            Assert.AreEqual(HttpStatusCode.NotFound, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void EnableCourse_ForbiddenRole_IsStatusCodeForbidden(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;
            
            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            api.Execute<bool>(disableCourseRequest);

            authenticator = api.GetAuthenticatorFor(role);
            RestRequest enableCourseRequest = api.InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute(enableCourseRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, actual.StatusCode, "Http Status Code");
        }

        [Test]
        public void EnableCourse_UnauthorizedUser_IsStatusCodeUnauthorized()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest disableCourseRequest = api.InitNewRequest("Disable course", Method.DELETE, authenticator);
            disableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            api.Execute<bool>(disableCourseRequest);

            var resource = ReaderUrlsJSON.ByName("Enable course", api.endpointsPath);
            var enableCourseRequest = new RestRequest(resource, Method.PATCH);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute(enableCourseRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode, "Http Status Code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void EnableCourse_CourseAlreadyActive_IsStatusCodeConflict(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateNameOf<Course>()));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            RestRequest enableCourseRequest = api.InitNewRequest("Enable course", Method.PATCH, authenticator);
            enableCourseRequest.AddUrlSegment("id", originCourse.Id.ToString());
            var actual = api.Execute(enableCourseRequest);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Conflict, actual.StatusCode, "Http Status Code");
                StringAssert.Contains("Course is already active", actual.Content, "Error message");
            });
        }
    }
}