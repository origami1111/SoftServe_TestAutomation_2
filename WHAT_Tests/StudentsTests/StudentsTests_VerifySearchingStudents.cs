using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System;
using WHAT_PageObject;
using System.Linq;

namespace WHAT_Tests
{
    [TestFixture]

    public class StudentsTests_VerifySearchingStudents
    {

        private StudentsPage studentsPage;
        private IWebDriver driver;

        [OneTimeSetUp]
        public  void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(ReaderUrlsJSON.ByName("SigninPage"));
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password);

        }

        [SetUp]
        public void ToStudentPage()
        {
            studentsPage.SidebarNavigateTo<StudentsPage>();
        }

        [Test]
        [TestCase("student", "Student")]
        public void FillSearchingField_ValidData( string forSearch)
        {
            studentsPage.FillSearchingField(forSearch);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
