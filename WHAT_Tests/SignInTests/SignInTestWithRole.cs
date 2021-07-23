using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [AllureNUnit]
    [TestFixture]
    class SignInTestWithRole : TestBase
    {
        private SignInPage signInPage;
        private Credentials credentials;
        private WebDriverWait wait;

        [SetUp]
        public void SetupPage()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            signInPage = new SignInPage(driver);
        }

        [Test]
        [TestCase(Role.Admin, "StudentsPage")]
        [TestCase(Role.Secretary, "MentorsPage")]
        [TestCase(Role.Mentor, "LessonsPage")]
        [TestCase(Role.Student, "SupportPage")]
        public void SignInWithRole(Role role, string url)
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            string expected = ReaderUrlsJSON.GetUrlByName(url, LinksPath);

            signInPage.SignInAsAdmin(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

    }
}
