using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using WHAT_PageObject;


namespace WHAT_Tests
{
    [TestFixture]
    public class SecretariesPaginationTests : TestBase
    {
        private SecretariesPage secretariesPage;

        [SetUp]
        public void Setup()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            
            secretariesPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>();
        }

        [TearDown]
        public void Down()
        {
            secretariesPage.Logout();
        }


        [TestCase(ShowedUsers.ten)]
        [TestCase(ShowedUsers.fifty)]
        [TestCase(ShowedUsers.oneHundred)]
        [Test]
        public void VerifyFirstPageCount(ShowedUsers usersOnPage)
        {
            int expected;
            secretariesPage.SelectUsersOnPage(usersOnPage);
            if (secretariesPage.GetLastUserIndex() >= secretariesPage.GetUsersOnPage())
            {
                expected = secretariesPage.GetUsersOnPage();
                secretariesPage.FirstPage();
            }
            else
            {
                expected = secretariesPage.GetLastUserIndex();
            }
            int actual = secretariesPage.GetShowedUsersAmount();
            Assert.AreEqual(expected, actual);
        }

    }
}
