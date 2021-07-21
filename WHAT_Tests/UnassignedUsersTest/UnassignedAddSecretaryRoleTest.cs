﻿using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class UnassignedAddSecretaryRoleTest : TestBase
    {
        private UnassignedUsersPage unassignedUsers;

        [SetUp]
        public void Setup()
        {
            var account = ReaderFileJson.ReadFileJsonAccounts(Role.Admin);

            unassignedUsers = new SignInPage(driver)
                            .SignInAsAdmin(account.Email, account.Password)
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
