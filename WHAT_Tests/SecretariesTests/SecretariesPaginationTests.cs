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

        [Test]        
        public void VerifyFirstPageCount10()
        {
            int expected;
            if (secretariesPage.GetReportedUsersTotal()/
                secretariesPage.GetUsersOnPage()>0)
            {
                expected = secretariesPage.GetUsersOnPage();
            }
            else
            {
                expected = secretariesPage.GetReportedUsersTotal();
            }

            int actual = secretariesPage.GetShowedUsersAmount();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyFirstPageCount100()
        {
            int expected;
            secretariesPage.SelectUsersOnPage(ShowedUsers.oneHundred);
            if (secretariesPage.GetReportedUsersTotal() /
                secretariesPage.GetUsersOnPage() > 0)
            {
                expected = secretariesPage.GetUsersOnPage();
            }
            else
            {
                expected = secretariesPage.GetReportedUsersTotal();
            }

            int actual = secretariesPage.GetShowedUsersAmount();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyLastPageCount10() 
        {
            int expected = secretariesPage.GetReportedUsersTotal() % 
                            secretariesPage.GetUsersOnPage();
            int actual = secretariesPage.LastPage().GetShowedUsersAmount();
            Assert.AreEqual(expected, actual);
        }

        //[Test]    Should to fix LastPage()
        //public void VerifyLastPageCount100()
        //{
        //    secretariesPage.SelectUsersOnPage(ShowedUsers.oneHundred);
        //    int expected = secretariesPage.GetReportedUsersTotal() %
        //                    secretariesPage.GetUsersOnPage();
        //    int actual = secretariesPage.LastPage().GetShowedUsersAmount();
        //    Assert.AreEqual(expected, actual);
        //}
    }
}
