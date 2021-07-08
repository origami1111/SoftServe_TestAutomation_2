using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsEdtTests: TestBase
    {

        private StudentsEditPage studentsEditPage;
        private StudentsPage studentsPage;

        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>();
        }

        [TearDown]
        public void Postcondition()
        {
            studentsPage.Logout();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
