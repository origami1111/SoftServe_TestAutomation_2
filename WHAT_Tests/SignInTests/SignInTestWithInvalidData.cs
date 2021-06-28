using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    class SignInTestWithInvalidData : TestBase
    {
        private SignInPage signInPage;

        [SetUp]
        public void SetupPage()
        {
            signInPage = new SignInPage(driver);
        }

        [Test]
        [TestCase("email", "password")]
        public void SignInAsWithInvalidData(string email, string password)
        {
            signInPage
                .FillEmail(email)
                .FillPassword(password)
                .ClickSignInButton();

            Assert.AreEqual("An error occurred", signInPage.GetErrorText());
        }

    }
}
