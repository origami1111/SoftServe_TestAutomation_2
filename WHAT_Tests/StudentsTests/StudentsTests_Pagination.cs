using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;


namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests_Pagination
    {
        private StudentsPage studentsPage;
        private IWebDriver driver;

        [OneTimeSetUp]
        public void SetUp()
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
        public void FillSearchingField_ValidData([Values("student", "Student")] string forSearch)
        {
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

    }
}
