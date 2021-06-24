using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;

namespace WHAT_Tests.RegistrationTests
{
    [TestFixture]
    class RegistrationTests
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
        public void RegistrationWithValidData()
        {

        }

        [Test]
        public void RedirectToSignInPage()
        {

        }
    }
}
