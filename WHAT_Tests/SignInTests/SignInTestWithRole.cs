using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using WHAT_Utilities;
using WHAT_PageObject;

namespace WHAT_Tests
{
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
        public void SignInAsAdmin()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            string expected = ReaderUrlsJSON.GetUrlByName("StudentsPage", LinksPath); 

            signInPage.SignInAsAdmin(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsSecretar()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Secretary);
            string expected = ReaderUrlsJSON.GetUrlByName("MentorsPage", LinksPath);

            signInPage.SignInAsSecretar(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsMentor()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);
            string expected = ReaderUrlsJSON.GetUrlByName("LessonsPage", LinksPath);

            signInPage.SignInAsMentor(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsStudent()
        {
            credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Student);
            string expected = ReaderUrlsJSON.GetUrlByName("SupportPage", LinksPath);

            signInPage.SignInAsStudent(credentials.Email, credentials.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

    }
}
