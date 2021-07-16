using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class UnassignedAddSecretaryRoleTest: TestBase
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
        }

        [Test]

        [TestCase(7)]
        public void AddRole(int secretaryID)
        {
            unassignedUsers.AddSecretaryRole(secretaryID);

            SecretariesPage secretariesPage = new SecretariesPage(driver)
                                          .SidebarNavigateTo<SecretariesPage>();

            driver.Navigate().Refresh();

            bool actual = unassignedUsers.UserVerify<SecretariesPage>(unassignedUsers.FirstName, unassignedUsers.LastName, unassignedUsers.Email);

            Assert.IsTrue(actual);
        }
    }
}
