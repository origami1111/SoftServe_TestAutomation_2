using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests_VerifySearchingStudents_Invalid : TestBase
    {
        private StudentsPage studentsPage;
        private static Credentials mentor = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);
        private static Credentials admin = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
        private static Credentials secretary = ReaderFileJson.ReadFileJsonCredentials(Role.Secretary);

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
        [TestCaseSource(nameof(InfoSource))]
        public void FillSearchingField_ValidData(int id, string firstName, string lastName)
        {
            studentsPage.FillSearchingField($@"{firstName} {lastName}");
            Dictionary<int, string[]> allStudentsInfo = studentsPage.GetStudentsFromTable();
            KeyValuePair<int, string[]> ourPair = new KeyValuePair<int, string[]>(id, new string[] { firstName, lastName });
            int expected = 1;
            int actual = 0;
            foreach (var item in allStudentsInfo)
            {
                if (item.Value[0] == ourPair.Value[0] && item.Value[1] == ourPair.Value[1])
                {
                    actual++;
                    break;
                }
            }
            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        [TestCaseSource(nameof(InfoSource))]
        public void FillSearchingField_ValidData_DisabledStudents(int id, string firstName, string lastName)
        {
            studentsPage.ClickDisabledStudents_CheckBox();
            FillSearchingField_ValidData(id, firstName, lastName);
        }

        public static IEnumerable<TestCaseData> InfoSource()
        {
            yield return new TestCaseData(new object[] { mentor.ID, mentor.FirstName, mentor.LastName });
            yield return new TestCaseData(new object[] { secretary.ID, secretary.FirstName, secretary.LastName });
            yield return new TestCaseData(new object[] { admin.ID, admin.FirstName, admin.LastName });
        }
    }
}
