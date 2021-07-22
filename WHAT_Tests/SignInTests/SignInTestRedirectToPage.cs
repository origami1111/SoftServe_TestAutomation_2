﻿using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [AllureNUnit]
    [TestFixture]
    class SignInTestRedirectToPage : TestBase
    {
        private SignInPage signInPage;

        [SetUp]
        public void SetupPage()
        {
            signInPage = new SignInPage(driver);
        }

        [Test]
        public void RedirectToRegistrationPage()
        {
            string expected = ReaderUrlsJSON.GetUrlByName("RegistrationPage", LinksPath);

            signInPage.ClickRegistrationLink();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }
    }
}
