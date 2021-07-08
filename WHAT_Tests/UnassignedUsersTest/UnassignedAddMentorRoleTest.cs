using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class UnassignedAddMentorRoleTest : TestBase
    {
        private UnassignedUsersPage unassignedUsers;

        [SetUp]
        public void Setup()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            unassignedUsers = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<UnassignedUsersPage>();
        }

        [TearDown]
        public void Down()
        {
            unassignedUsers.Logout();
            driver.Quit();
        }

        [Test]

        [TestCase(5)]
        public void AddRole(int mentorID)
        {
            string expected = unassignedUsers.AddMentorRole(mentorID);

            MentorsPage mentorsPage = new MentorsPage(driver)
                                          .SidebarNavigateTo<MentorsPage>();
        }
    }
}
