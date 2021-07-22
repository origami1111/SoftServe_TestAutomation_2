using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [AllureNUnit]
    [TestFixture]
    public class StudentsTests_VerifyRedirection : TestBase
    {
        private StudentsPage studentsPage;

        public StudentsTests_VerifyRedirection()
        {
            log = LogManager.GetLogger($"StudentsPage/{nameof(StudentsTests_VerifyRedirection)}");
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
        [TestCase(1)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(17)]
        [TestCase(20)]
        public void RedirectStudentsEdit_AnyCard(int studentNum)
        {
            List<string[]> allStudentsInfo = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {allStudentsInfo.Count}");
            var expecteStudentInfo = allStudentsInfo.ElementAt(studentNum-1);
            StudentDetailsPage studentsEdit =studentsPage.ClickChoosedStudent(studentNum);
            log.Info($"Click on {studentNum} student card");
            string[] actualStudenInfo = studentsEdit.GetTexFromAllField();
            CollectionAssert.AreEqual(expecteStudentInfo, actualStudenInfo);
        }
    }
}
