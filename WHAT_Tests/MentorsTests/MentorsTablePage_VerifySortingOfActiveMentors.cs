using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class MentorsTablePage_VerifySortingOfActiveMentors : TestBase
    {
        private MentorsPage mentorsPage;

        [SetUp]
        public void Precondition()
        {
            log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            mentorsPage.Logout();
        }

        [Test, Description("DP213-67")]
        public void TestMentorsTablePage_VerifySortingOfActiveMentors()
        {
            var adminCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            var secretaryCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Secretary);
            string entryCount = "99";

            mentorsPage = new SignInPage(driver)
                .SignInAsAdmin(adminCredentials.Email, adminCredentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .SelectFromRowAmountDropdown(entryCount)
                .ClickSortByFirstName()
                .VerifyCorrectSorftingByFirstNameAsc()
                .ClickSortByFirstName()
                .VerifyCorrectSorftingByFirstNameDesc()
                .ClickSortByLastName()
                .VerifyCorrectSorftingByLastNameAsc()
                .ClickSortByLastName()
                .VerifyCorrectSorftingByLastNameDesc()
                .ClickSortByEmail()
                .VerifyCorrectSorftingByEmailAsc()
                .ClickSortByEmail()
                .VerifyCorrectSorftingByEmailDesc()
                ;
        }
    }
}
