using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;

namespace WHAT_DP_205_TAQC
{
    [TestFixture]
    public class WHAT_CoursesTests
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
            
            var signInPage = new SignInPage(driver);
            var lessonsPage = signInPage.LoginAsMentor();
            
            coursesPage = lessonsPage.OpenCoursesPage();
        }

        [TearDown]
        public void Logout()
        {
            
        }
        
        [Test]
        public void VerifyCourseDetails()
        {
            string courseNumber = "1";
            string expectedText =  coursesPage.ReadCourseName(courseNumber);
            
            var courseDetailsComponent = coursesPage.ClickCourseName(courseNumber);
            string actualText = courseDetailsComponent.ReadCourseNameDetails();

            Assert.AreEqual(expectedText, actualText);
        }
    }
}