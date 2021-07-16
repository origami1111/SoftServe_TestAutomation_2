using NUnit.Framework;
using RestSharp;
using System;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class Courses_Tests : API_BaseTest
    {
        private static string GenerateRandomCourseName() =>
            $"Test course {Guid.NewGuid():N}";
        /*
        [Test]
        public void VerifyCourseDetails()
        {
            int courseNumber = 1;
            string expected = coursesPage.ReadCourseName(courseNumber);

            var courseDetailsPage = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsPage.GetCourseNameDetails();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EditCourse_CLickClearButton()
        {
            string expected = coursesPage.ReadCourseName();

            var actual = coursesPage.ClickEditIcon()
                                    .DeleteTextWithBackspaces(expected.Length)
                                    .ClickClearButton()
                                    .GetCourseName();

            Assert.AreEqual(expected, actual);
        }
        */

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void AddCourse_ValidData(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);

            string expected = GenerateRandomCourseName();

            // POST
            RestRequest postRequest = InitNewRequest("POST Adds new course", Method.POST, authenticator);
            postRequest.AddJsonBody(new CreateCourse { Name = expected });
            var actual = Execute<Course>(postRequest);

            Assert.AreEqual(expected, actual.Name);

            // DELETE
            RestRequest deleteRequest = InitNewRequest("DELETE Disable courses",
                Method.DELETE, authenticator);
            deleteRequest.AddUrlSegment("id", actual.Id.ToString());
            Execute<bool>(deleteRequest);
        }

/*
        [TestCase("a", "Too short")]
        [TestCase("Course name with more than 50 characters is too long", "Too long")]
        [TestCase(" Space before course name", "Invalid course name")]
        [TestCase("More than one space   between words", "Invalid course name")]
        [TestCase("Space after course name ", "Invalid course name")]
        [TestCase("Course name with special symbols //", "Invalid course name")]
        [TestCase("Not only Latin letters Кириллица", "Invalid course name")]
        public void AddCourse_InvalidData_isErrorMessageDisplayed(string invalidData, string expected)
        {
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(invalidData)
                                    .GetErrorMessage();

            Assert.AreEqual(expected, actual);
        }

        [TestCase("a")]
        [TestCase("Course name with more than 50 characters is too long")]
        [TestCase(" Space before course name")]
        [TestCase("More than one space   between words")]
        [TestCase("Space after course name ")]
        [TestCase("Course name with special symbols //")]
        [TestCase("Not only Latin letters Кириллица")]
        public void AddCourse_InvalidData_IsSaveButtonDisabled(string invalidData)
        {
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(invalidData)
                                    .IsSaveButtonDisabled();

            Assert.True(actual);
        }

        [Test]
        public void AddCourse_EmptyName_isErrorMessageDisplayed()
        {
            var expected = "This field is required";

            var anyData = GenerateRandomCourseName();
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(anyData)
                                    .DeleteTextWithBackspaces(anyData.Length)
                                    .GetErrorMessage();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddCourse_EmptyName_IsSaveButtonDisabled()
        {
            var anyData = GenerateRandomCourseName();
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(anyData)
                                    .DeleteTextWithBackspaces(anyData.Length)
                                    .IsSaveButtonDisabled();

            Assert.True(actual);
        }
        
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        public void Test(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);

            var requestData = File.ReadAllText("JsonDataFiles/CreateSchedule.json");
            var expected = JsonConvert.DeserializeObject<CreateSchedule>(requestData);
            
            // POST
            RestRequest postRequest = InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(requestData);
            var originalSchedule = Execute<EventOccurrence>(postRequest);
            
            // PUT
            RestRequest putRequest = InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            requestData = File.ReadAllText("JsonDataFiles/UpdateSchedule.json");
            putRequest.AddJsonBody(requestData);
            var actualSchedule = Execute<EventOccurrence>(putRequest);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Pattern.Type, actualSchedule.Pattern, "");
                Assert.AreEqual(expected.Context.GroupID, actualSchedule.StudentGroupId);
                CollectionAssert.AreEquivalent(originalSchedule.Events, actualSchedule.Events, "Updated Events");
            });
            
            // DELETE
            RestRequest deleteRequest = InitNewRequest("ApiSchedulesEventOccurrenceID",
                Method.DELETE, authenticator);
            deleteRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            var deleteSchedule = Execute<EventOccurrence>(deleteRequest);
        }
*/
    }
}