using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests_VerifyRedirection : TestBase
    {

        private StudentsPage studentsPage;

        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>();

        }

        [Test]
        public void RedirectUnassignedUsers()
        {
            studentsPage.ClickAddStudentButton();
            string unassignedUsersURL = ReaderUrlsJSON.GetUrlByName("UnassignedUsersPage", LinksPath);
            Assert.AreEqual(unassignedUsersURL, driver.Url);
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void RedirectStudentsEdit_AnyCard(int studentNum)
        {
            studentsPage.ClickChoosedStudent(studentNum);
            string studentEditURL = ReaderUrlsJSON.GetUrlByNameAndNumber("StudentsPage", studentNum, LinksPath);
            Assert.AreEqual(studentEditURL, driver.Url);
        }

    }
}
