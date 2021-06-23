using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageFactory;

namespace WHAT_Tests
{
    [TestFixture]
    public class FactoryCoursesTests
    {
        private IWebDriver driver;

        private CoursesPage coursesPage;

        [OneTimeSetUp]
        public void BeforeAllMethod()
        {
            driver = new ChromeDriver();
        }

        [OneTimeTearDown]
        public void AfterAllMethod()
        {
            driver.Quit();
        }

        [SetUp]
        public void Setup()
        {
            driver.Navigate().GoToUrl("http://localhost:8080/auth");

            coursesPage = new SignInPage(driver)
                .SignInAsMentor()
                .SidebarNavigateTo<CoursesPage>();
        }

        [TearDown]
        public void Logout()
        {

        }

        [Test]
        public void VerifyCourseDetails()
        {
            string expectedText = coursesPage.ReadCourseName();

            string actualText = coursesPage.ClickCourseName()
                                           .ReadCourseNameDetails();

            Assert.AreEqual(expectedText, actualText);
        }
    }
}