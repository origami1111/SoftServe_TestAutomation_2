using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [AllureNUnit]
    [TestFixture]
    public class StudentsTests_VerifySearchingStudents_Valid : TestBase
    {
        private StudentsPage studentsPage;
        private static Credentials studentInfo = ReaderFileJson.ReadFileJsonCredentials(Role.Student);

        public StudentsTests_VerifySearchingStudents_Valid()
        {
            log = LogManager.GetLogger($"StudentsPage/{nameof(StudentsTests_VerifySearchingStudents_Valid)}");
        }

        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>();
            log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            studentsPage.Logout();
        }

        [Test]
        [TestCaseSource(nameof(StudentInfoSource))]
        public void FillSearchingField_ValidData( string firstName, string lastName)
        {
            studentsPage.FillSearchingField($@"{firstName} {lastName}");
            log.Info($"Fill int field {firstName} and {lastName}");
            List<string[]> allStudentsInfo = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {allStudentsInfo.Count}");
            string[] ourPair =  new string[] { firstName, lastName };
            int expected = 1;
            int actual = 0;
            foreach (var item in allStudentsInfo)
            {
                if (item[0] == ourPair[0] && item[1] == ourPair[1])
                {
                    actual++;
                    break;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<TestCaseData> StudentInfoSource()
        {
            yield return new TestCaseData(new object[] { studentInfo.FirstName, studentInfo.LastName });
        }
    }
}
