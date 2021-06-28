using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests.SignInTests
{
    [TestFixture]
    class SignInTestWithInvalidData
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
        [TestCase("email", "password")]
        public void SignInAsWithInvalidData(string email, string password)
        {
            signInPage.
                FillEmail(email).
                FillPassword(password).
                ClickSignInButton();

            Assert.AreEqual("An error occurred", signInPage.GetErrorText());
        }
    }
}
