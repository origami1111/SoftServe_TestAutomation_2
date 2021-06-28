using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests1
    {
        private IWebDriver driver;

        private IConfigurationRoot configuration;

        private CoursesPage coursesPage;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            
            var url = configuration["Url"];
            
            var credential = configuration.GetSection("Credentials");

            var admin = new Credentials()
            {
                Email = credential["Admin:Email"],
                Password = credential["Admin:Password"]
            };
            
            var mentor = new Credentials()
            {
                Email = credential["Mentor:Email"],
                Password = credential["Mentor:Password"]
            };
            
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            coursesPage = new SignIn(driver)
                            .SignInAsAdmin()
                            .SidebarNavigateTo<CoursesPage>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            coursesPage.Logout();
            driver.Quit();
        }

        [Test]
        public void VerifyCourseDetails()
        {
            int courseNumber = 3;
            string expected =  coursesPage.ReadCourseName(courseNumber);
            
            var courseDetailsPage = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsPage.GetCourseNameDetails();
            driver.Navigate().Back();

            Assert.AreEqual(expected, actual);

        }

        /*
        [Test]
        public void EditCourse()
        {
            int courseNumber = 3;
            string courseName = "New course";
            coursesPage.ClickPencilLink(courseNumber)
                       .FillCourseName(courseName)
                       .ClickCancelButton();

        }*/

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