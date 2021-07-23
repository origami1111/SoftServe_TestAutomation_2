using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class UnassignedAddStudentRoleTest : TestBase
    {
        private UnassignedUsersPage unassignedUsers;

        [SetUp]
        public void PreCondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            unassignedUsers = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<UnassignedUsersPage>();
        }

        [TearDown]
        public void PostCondition()
        {
            unassignedUsers.Logout();
        }

        [Test]
        [TestCase(1)]
        public void AddRole(int studentID)
        {
            unassignedUsers.AddStudentRole(studentID);

            StudentsPage studentsPage = new StudentsPage(driver)
                              .SidebarNavigateTo<StudentsPage>();
            driver.Navigate().Refresh();

            bool actual = unassignedUsers.UserVerify<StudentsPage>(unassignedUsers.FirstName, unassignedUsers.LastName, unassignedUsers.Email);

            Assert.IsTrue(actual);
        }
    }
}
