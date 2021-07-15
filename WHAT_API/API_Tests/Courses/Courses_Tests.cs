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
        private static string GenerateCourseName() =>
            $"Test course {Guid.NewGuid().ToString("N")}";
        
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        public void Courses_AddNew_ValidCourseName(Role role)
        {
            var expected = new CreateOrUpdateCourse(GenerateCourseName());
            
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

            // DELETE
            RestRequest deleteRequest = InitNewRequest("Disable course",
                Method.DELETE, authenticator);
            deleteRequest.AddUrlSegment("id", actual.Id.ToString());
            Execute<bool>(deleteRequest);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        public void Courses_AddNew_SameCourseName(Role role)
        {
            var expected = HttpStatusCode.Conflict;
            
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateCourseName()));
            client.Execute(addCourseRequest);
            var actual = client.Execute(addCourseRequest).StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void Courses_AddNew_ForbiddenStatusCode(Role role)
        {
            var expected = HttpStatusCode.Forbidden;
            
            var authenticator = GetAuthenticatorFor(role);
            RestRequest addCourseRequest = InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateCourseName()));
            var actual = client.Execute(addCourseRequest).StatusCode;

            Assert.AreEqual(expected, actual);
        }

        public void Courses_AddNew_UnauthorizedStatusCode()
        {
            var expected = HttpStatusCode.Unauthorized;
            
            var resource = ReaderUrlsJSON.ByName("Add new course", endpointsPath);
            var addCourseRequest = new RestRequest(resource, Method.POST);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse(GenerateCourseName()));
            var actual = client.Execute(addCourseRequest).StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Role.Admin, true)]
        [TestCase(Role.Admin, false)]
        [TestCase(Role.Secretar, true)]
        [TestCase(Role.Secretar, false)]
        [TestCase(Role.Mentor, true)]
        [TestCase(Role.Mentor, false)]
        [TestCase(Role.Student, true)]
        [TestCase(Role.Student, false)]
        public void Courses_Get_ActiveNotActive(Role role, bool isActive)
        {
            var authenticator = GetAuthenticatorFor(role);
            RestRequest getCoursesRequest = InitNewRequest("Get courses", Method.GET, authenticator);
            getCoursesRequest.AddQueryParameter("isActive", isActive.ToString());
            var actualCourses = Execute<List<Course>>(getCoursesRequest);
            System.Diagnostics.Debug.WriteLine(actualCourses.Count);

            Assert.NotNull(actualCourses);
            Assert.Multiple(() =>
            {
                CollectionAssert.IsNotEmpty(actualCourses);
                Assert.True(actualCourses.All(course => course.IsActive == isActive));
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void Courses_Get_All(Role role)
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
    }
}