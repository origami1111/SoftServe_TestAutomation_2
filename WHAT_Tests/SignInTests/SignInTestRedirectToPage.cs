using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests.SignInTests
{
    [TestFixture]
    class SignInTestRedirectToPage
    {
        private IWebDriver driver;
        private SignInPage signInPage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
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
        public void RedirectToRegistrationPage()
        {
            string expected = "http://localhost:8080/registration";

            signInPage.ClickRegistrationLink();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }
    }
}
