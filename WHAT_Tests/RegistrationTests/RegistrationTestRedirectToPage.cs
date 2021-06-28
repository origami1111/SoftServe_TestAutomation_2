using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests.RegistrationTests
{
    [TestFixture]
    class RegistrationTestRedirectToPage
    {
        private IWebDriver driver;
        private RegistrationPage registrationPage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl("http://localhost:8080/registration");

            registrationPage = new RegistrationPage(driver);
        }

        [TearDown]
        public void Logout()
        {
            driver.Quit();
        }

        [Test]
        public void RedirectToSignInPage()
        {
            string expected = "http://localhost:8080/auth";

            registrationPage.ClickLogInLink();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }
    }
}
