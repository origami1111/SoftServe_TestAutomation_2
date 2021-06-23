using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests
    {
        private IWebDriver driver;

        private CoursesPage coursesPage;
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:8080/auth");

            coursesPage = new SignInPage(driver)
                            .SignInAsMentor("mentor@gmail.com", "What_123")
                            .SidebarNavigateTo<CoursesPage>().ClickCourseName("5")
                            .SidebarNavigateTo<LessonsPage>()
                            .SidebarNavigateTo<CoursesPage>().ClickCourseName("3").SidebarNavigateTo<CoursesPage>();
        }

        [TearDown]
        public void Logout()
        {
            driver.Quit();
        }

        [Test]
        public void VerifyCourseDetails()
        {
            string courseNumber = "1";
            string expected =  coursesPage.ReadCourseName(courseNumber);
            
            var courseDetailsComponent = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsComponent.ReadCourseNameDetails();

            Assert.AreEqual(expected, actual);
        }
    }
}