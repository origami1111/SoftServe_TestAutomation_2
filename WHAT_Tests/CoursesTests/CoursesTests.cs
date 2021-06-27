using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests : TestBase
    {
        private CoursesPage coursesPage;

        [SetUp]
        public void Setup()
        {
            coursesPage = new SignIn(driver)
                            .SignInAsAdmin()
                            .SidebarNavigateTo<CoursesPage>();
        }

        [TearDown]
        public void TearDown()
        {
            coursesPage.Logout();
        }

        [Test]
        public void VerifyCourseDetails()
        {
            int courseNumber = 3;
            string expected = coursesPage.ReadCourseName(courseNumber);

            var courseDetailsPage = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsPage.ReadCourseNameDetails();
            driver.Navigate().Back();

            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void EditCourse()
        {
            int courseNumber = 3;
            string courseName = "New course";
            coursesPage.ClickPencilLink(courseNumber);
            //  .FillCourseName(courseName)
            //  .ClickCancelButton();

        }


        [Test]
        public void AddCourse()
        {
            string courseName = "New course";
            coursesPage.ClickAddCourseButton()
                       .FillCourseName(courseName)
                       .ClickCancelButton();

        }
    }
}