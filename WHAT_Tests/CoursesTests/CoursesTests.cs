using NUnit.Framework;
using NUnit.Framework.Internal;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests : TestBase
    {
        private CoursesPage coursesPage;

        [SetUp]
        public void Precondition()
        {

            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            coursesPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
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
            string expected =  coursesPage.ReadCourseName(courseNumber);
            
            var courseDetailsPage = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsPage.GetCourseNameDetails();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EditCourse_CLickClearButton()
        {
            int courseNumber = 1;
            string expected = coursesPage.ReadCourseName(courseNumber);

            var actual = coursesPage.ClickEditIcon(courseNumber)
                                    .DeleteTextWithBackspaces()
                                    .ClickClearButton()
                                    .GetCourseName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddCourse_ValidData()
        {
            string courseName = StringRandomizer.GetRandomCourseName();
            coursesPage.ClickAddCourseButton()
                       .FillCourseNameField(courseName)
                       .ClickCancelButton();
        }

        [TestCase("a", "Too short")]
        [TestCase("Course name with more than 50 characters is too long", "Too long")]
        [TestCase(" Space before course name", "Invalid course name")]
        [TestCase("More than one space   between words", "Invalid course name")]
        [TestCase("Space after course name ", "Invalid course name")]
        [TestCase("Course name with special symbols: C#,/ .Net", "Invalid course name")]
        [TestCase("Not only Latin letters Кириллица", "Invalid course name")]
        public void AddCourseWithInvalidData(string invalidData, string expected)
        {
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(invalidData);
            
            Assert.True(expected == actual.GetErrorMessage() && actual.IsSaveButtonDisabled());
        }

        [Test]
        public void AddCourse_EmptyName()
        {
            var expected = "This field is required";
            var anyData = "Test";
            
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(anyData)
                                    .DeleteTextWithBackspaces(anyData.Length);
            
            Assert.True(expected == actual.GetErrorMessage() && actual.IsSaveButtonDisabled());
        }
    }
}