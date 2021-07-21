using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests : TestBase
    {
        private CoursesPage coursesPage;

        private readonly Account account = ReaderFileJson.ReadFileJsonAccounts(Role.Secretary);

        private static string GenerateRandomCourseName() =>
            $"Test course {Guid.NewGuid():N}";

        [SetUp]
        public void Precondition()
        {
            coursesPage = new SignInPage(driver)
                            .SignInAsAdmin(account.Email, account.Password)
                            .SidebarNavigateTo<CoursesPage>();
        }

        [TearDown]
        public void Postcondition()
        {
            coursesPage.Logout();
        }

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

        [Test]
        public void AddCourse_ValidData()
        {
            string expected = GenerateRandomCourseName();

            string actual = coursesPage.ClickAddCourseButton()
                                       .FillCourseNameField(expected)
                                       .ClickSaveButton()
                                       .FillSearchField(expected)
                                       .ReadCourseName();

            Assert.AreEqual(expected, actual);
        }

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
    }
}