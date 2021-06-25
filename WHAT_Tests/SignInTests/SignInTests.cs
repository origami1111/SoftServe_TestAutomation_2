using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using WHAT_PageObject;
using WHAT_PageObject.Base;

namespace WHAT_Tests.SignInTests
{
    [TestFixture]
    class SignInTests
    {
        private IWebDriver driver;
        private SignIn signIn;
        private SignInPage signInPage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:8080/auth");

            signIn = new SignIn(driver);
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
            StudentsPage studentsPage = signIn.SignInAsAdmin();

            Thread.Sleep(3000);

            Assert.AreEqual("http://localhost:8080/students", driver.Url);
        }

        [Test]
        public void SignInAsSecretar()
        {
            signIn.SignInAsSecretar();

            Thread.Sleep(3000);

            Assert.AreEqual("http://localhost:8080/mentors", driver.Url);
        }

        [Test]
        public void SignInAsMentor()
        {
            signIn.SignInAsMentor();

            Thread.Sleep(3000);

            Assert.AreEqual("http://localhost:8080/lessons", driver.Url);
        }

        [Test]
        public void SignInAsStudent()
        {
            signIn.SignInAsStudent();

            Thread.Sleep(3000);

            Assert.AreEqual("http://localhost:8080/support", driver.Url);
        }

        [Test]
        [TestCase("email", "password")]
        public void SignInAsWithInvalidData(string email, string password)
        {
            signInPage.
                FillEmail(email).
                FillPassword(password).
                ClickSignInButton();

            Thread.Sleep(3000);

            Assert.AreEqual("An error occurred", signInPage.GetAnErrorOccured());
        }

        [Test]
        public void RedirectToRegistrationPage()
        {
            signInPage.ClickRegistrationLink();

            Thread.Sleep(3000);

            Assert.AreEqual("http://localhost:8080/registration", driver.Url);
        }
    }
}
