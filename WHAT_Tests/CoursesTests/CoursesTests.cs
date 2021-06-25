using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests
    {
        private IWebDriver driver;

        private IConfigurationRoot configuration;

        private CoursesPage coursesPage;


        [SetUp]
        public void Setup()
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
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            coursesPage = new SignInPage(driver)
                            .SignInAsMentor(mentor.Email, mentor.Password)
                            .SidebarNavigateTo<CoursesPage>();
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

        [Test]
        public void AddCourse()
        {



            
        }
    }
}