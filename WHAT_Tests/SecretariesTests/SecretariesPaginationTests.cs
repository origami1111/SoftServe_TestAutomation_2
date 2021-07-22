using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using WHAT_PageObject;
using WHAT_Utilities;

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
        public void VerifyFirstPageCount(ShowedUsers usersAtPage)
        {
            int expected;
            int lastUserIndex;
            secretariesPage.SelectUsersAtPage(usersAtPage);
            int selectedUsersAtPage;
            int actual = secretariesPage.GetShowedUsersAmount();

            if (secretariesPage.GetLastUserIndex(out lastUserIndex) && secretariesPage.GetUsersAtPage(out selectedUsersAtPage))
            {

                if (lastUserIndex >= selectedUsersAtPage)
                {
                    expected = selectedUsersAtPage;
                }
                else
                {
                    expected = lastUserIndex;
                }

            }
            else
            {
                expected = 0;
                Assert.Fail();
            }

            Assert.AreEqual(expected, actual);
        }

        [TestCase(ShowedUsers.ten)]
        [TestCase(ShowedUsers.fifty)]
        [TestCase(ShowedUsers.oneHundred)]
        [Test]
        public void VerifyLastPageCount(ShowedUsers usersAtPage)
        {
            secretariesPage.SelectUsersAtPage(usersAtPage);
            int expected;
            int lastUserIndex;
            int selectedUsersAtPage;
            
            if (secretariesPage.GetUsersAtPage(out selectedUsersAtPage) && secretariesPage.GetLastUserIndex(out lastUserIndex))
            {
                expected = lastUserIndex % selectedUsersAtPage;
            }
            else
            {
                expected = 0;
                Assert.Fail();
            }
            
            int actual = secretariesPage.GetShowedUsersAmount();
            Assert.AreEqual(expected, actual);
            
        }

        [TestCase(ShowedUsers.ten)]
        [TestCase(ShowedUsers.fifty)]
        [TestCase(ShowedUsers.oneHundred)]
        [Test]
        public void VerifyMidlePageCount(ShowedUsers usersAtPage)
        {
            secretariesPage.SelectUsersAtPage(usersAtPage);
            int pagesAmount;
            int selectedUsersAtPage;

            if (secretariesPage.GetPagesAmount(out pagesAmount) && (pagesAmount > 2) && secretariesPage.GetUsersAtPage(out selectedUsersAtPage))
            {
                int expected = (int)usersAtPage;
                int actual = secretariesPage.PrevPage().GetShowedUsersAmount();
                Assert.AreEqual(expected, actual);
            }
            else
            {
                
            }
        }

    }
}
