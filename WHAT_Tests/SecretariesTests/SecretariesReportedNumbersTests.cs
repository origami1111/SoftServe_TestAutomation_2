﻿using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class SecretariesReportedNumbersTests : TestBase
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
        public void ReportedTotalVerify()
        {
            int actual = secretariesPage.GetReportedUsersTotal();
            int expected;

            if (secretariesPage.LastPage().GetLastUserIndex(out expected))
            {
                Assert.IsTrue(expected == actual);
            }
            else
            {
                Assert.Fail();
            }

        }

        [Test]
        public void ReportedAtFirstPageVerify()
        {
            int expected = secretariesPage.GetShowedUsersAmount();
            int actual = secretariesPage.GetReportedUsersAtPage();
            Assert.IsTrue(expected == actual);
        }

        [Test]
        public void ReportedAtLastPageVerify()
        {
            int expected = secretariesPage.LastPage().GetShowedUsersAmount();
            int actual = secretariesPage.GetReportedUsersAtPage();
            Assert.IsTrue(expected == actual);
        }
    }
}
