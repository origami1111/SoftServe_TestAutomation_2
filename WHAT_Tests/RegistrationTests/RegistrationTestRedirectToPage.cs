using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
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

            Thread.Sleep(3000);
            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }
    }
}
