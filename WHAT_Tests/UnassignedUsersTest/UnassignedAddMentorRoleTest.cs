using NUnit.Framework;
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

        [TestCase(5)]
        public void AddRole(int mentorID)
        {
            unassignedUsers.AddMentorRole(mentorID);

            MentorsPage mentorsPage = new MentorsPage(driver)
                                          .SidebarNavigateTo<MentorsPage>();

            driver.Navigate().Refresh();

            bool actual = unassignedUsers.UserVerify<MentorsPage>(unassignedUsers.FirstName, unassignedUsers.LastName, unassignedUsers.Email);

            Assert.IsTrue(actual);
        }
    }
}
