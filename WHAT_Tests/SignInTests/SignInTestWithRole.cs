using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WHAT_PageObject;

namespace WHAT_Tests.SignInTests
{
    [TestFixture]
    class SignInTestWithRole
    {
        private IWebDriver driver;
        private SignInPage signInPage;
        private Credentials credentials;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl("http://localhost:8080/auth");

            signInPage = new SignInPage(driver);
        }

        [TearDown]
        public void Logout()
        {
            driver.Quit();
        }

        [Test]
        public void SignInAsAdmin()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(@"DataFiles\Credentials.json", Role.Admin);
            string expected = "http://localhost:8080/students";

            signInPage.SignInAsAdmin(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsSecretar()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(@"DataFiles\Credentials.json", Role.Secretar);
            string expected = "http://localhost:8080/mentors";

            signInPage.SignInAsSecretar(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsMentor()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(@"DataFiles\Credentials.json", Role.Mentor);
            string expected = "http://localhost:8080/lessons";

            signInPage.SignInAsMentor(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsStudent()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(@"DataFiles\Credentials.json", Role.Student);
            string expected = "http://localhost:8080/support";

            signInPage.SignInAsStudent(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

    }
}
