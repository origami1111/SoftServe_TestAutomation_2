using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class MentorsPage_VerifySortingOfActiveMentors : TestBase
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
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]

        public void TestMentorsTablePage_VerifySortingOfActiveMentors(Role role)
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            string entryCount = "99";

            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
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
                .VerifyCorrectSorftingByEmailDesc();
        }
    }
}
