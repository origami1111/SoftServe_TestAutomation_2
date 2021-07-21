using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    class SignInTestWithRole : TestBase
    {
        private SignInPage signInPage;
        private Account account;
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
            account = ReaderFileJson.ReadFileJsonAccounts(Role.Admin);
            string expected = ReaderUrlsJSON.GetUrlByName("StudentsPage", LinksPath);

            signInPage.SignInAsAdmin(account.Email, account.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsSecretar()
        {
            account = ReaderFileJson.ReadFileJsonAccounts(Role.Secretary);
            string expected = ReaderUrlsJSON.GetUrlByName("MentorsPage", LinksPath);

            signInPage.SignInAsSecretar(account.Email, account.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsMentor()
        {
            account = ReaderFileJson.ReadFileJsonAccounts(Role.Mentor);
            string expected = ReaderUrlsJSON.GetUrlByName("LessonsPage", LinksPath);

            signInPage.SignInAsMentor(account.Email, account.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SignInAsStudent()
        {
            account = ReaderFileJson.ReadFileJsonAccounts(Role.Student);
            string expected = ReaderUrlsJSON.GetUrlByName("SupportPage", LinksPath);

            signInPage.SignInAsStudent(account.Email, account.Password);

            wait.Until(d => d.Url == expected);

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

    }
}
