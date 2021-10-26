using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class PUT_UpdateSchedule_Tests : API_BaseTest
    {
        private CreateSchedule createSchedule;

        [OneTimeSetUp]
        public void GetRelativeMonthlySchedule()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            var addCourseRequest = api.InitNewRequest("Add new course", Method.POST, authenticator);
            addCourseRequest.AddJsonBody(new CreateOrUpdateCourse($"Test Course {Guid.NewGuid():N}"));
            var originCourse = api.Execute<Course>(addCourseRequest).Data;

            var addStudentGroupRequest = api.InitNewRequest("ApiStudentsGroup", Method.POST, authenticator);
            var studentGroup = new CreateStudentGroup { CourseId = originCourse.Id };
            addStudentGroupRequest.AddJsonBody(studentGroup);
            var originstudentGroup = api.Execute<StudentGroup>(addStudentGroupRequest).Data;

            var pattern = PatternGenerator.GetRelativeMonthlyPattern(1, MonthIndex.Second,
                DayOfWeek.Thursday, DayOfWeek.Friday);
            createSchedule = new ScheduleGenerator().GenerateShedule(pattern, DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(1), originstudentGroup.Id);
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateSchedule_ValidData_IsSuccess(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest postRequest = api.InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(createSchedule);
            var originalSchedule = api.Execute<EventOccurrence>(postRequest).Data;

            createSchedule.Range.FinishDate = DateTime.UtcNow.AddMonths(2);
            RestRequest putRequest = api.InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            putRequest.AddJsonBody(createSchedule);
            var actualSchedule = api.Execute<EventOccurrence>(putRequest).Data;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(createSchedule.Pattern.Type, actualSchedule.Pattern, "Pattern");
                Assert.AreEqual(createSchedule.Context.GroupID, actualSchedule.StudentGroupId, "GroupID");
                CollectionAssert.AreEquivalent(originalSchedule.Events, actualSchedule.Events, "Updated Events");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateSchedule_SameSchedule_IsScheduleSame(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest postRequest = api.InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(createSchedule);
            var originalSchedule = api.Execute<EventOccurrence>(postRequest).Data;

            RestRequest putRequest = api.InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            putRequest.AddJsonBody(createSchedule);
            var actualSchedule = api.Execute<EventOccurrence>(putRequest).Data;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(createSchedule.Pattern.Type, actualSchedule.Pattern, "Pattern");
                Assert.AreEqual(createSchedule.Context.GroupID, actualSchedule.StudentGroupId, "GroupID");
                CollectionAssert.AreEquivalent(originalSchedule.Events, actualSchedule.Events, "Updated Events");
            });
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateSchedule_IncorrectScheduleId_IsStatusCodeNotFound(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest putRequest = api.InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", long.MaxValue);
            putRequest.AddJsonBody(createSchedule);
            var actualSchedule = api.Execute(putRequest);

            Assert.AreEqual(HttpStatusCode.NotFound, actualSchedule.StatusCode, "Http Status code");
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void UpdateSchedule_WithoutData_IsStatusCodeBadRequest(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(role);
            RestRequest postRequest = api.InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(createSchedule);
            var originalSchedule = api.Execute<EventOccurrence>(postRequest).Data;

            RestRequest putRequest = api.InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            putRequest.AddJsonBody(String.Empty);
            var actualSchedule = api.Execute(putRequest);

            Assert.AreEqual(HttpStatusCode.BadRequest, actualSchedule.StatusCode, "Http Status code");
        }

        [TestCase(Role.Mentor)]
        [TestCase(Role.Student)]
        public void UpdateSchedule_ForbiddenRole_IsStatusCodeForbidden(Role role)
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest postRequest = api.InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(createSchedule);
            var originalSchedule = api.Execute<EventOccurrence>(postRequest).Data;

            authenticator = api.GetAuthenticatorFor(role);
            RestRequest putRequest = api.InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            putRequest.AddJsonBody(createSchedule);
            var actualSchedule = api.Execute(putRequest);
            
            Assert.AreEqual(HttpStatusCode.Forbidden, actualSchedule.StatusCode, "Http Status code");
        }

        [Test]
        public void UpdateSchedule_UnauthorizedUser_IsStatusCodeUnauthorized()
        {
            var authenticator = api.GetAuthenticatorFor(Role.Admin);
            RestRequest postRequest = api.InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(createSchedule);
            var originalSchedule = api.Execute<EventOccurrence>(postRequest).Data;

            var resource = ReaderUrlsJSON.ByName("ApiSchedulesEventOccurrences-eventOccurrenceID", api.endpointsPath);
            var putRequest = new RestRequest(resource, Method.PUT);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            putRequest.AddJsonBody(createSchedule);
            var actualSchedule = api.Execute(putRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, actualSchedule.StatusCode, "Http Status code");
        }
    }
}