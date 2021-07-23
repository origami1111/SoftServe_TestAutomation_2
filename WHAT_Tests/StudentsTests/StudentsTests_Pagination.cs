using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class StudentsTests_Pagination : TestBase
    {
        private StudentsPage studentsPage;

        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>();
            log.Info($"Go to {driver.Url}");
        }

        public StudentsTests_Pagination()
        {
            log = LogManager.GetLogger($"StudentsPage/{nameof(StudentsTests_Pagination)}");
        }

        [TearDown]
        public void Postcondition()
        {
            studentsPage.Logout();
        }

        [Test]
        public void VerifyPagination_PrevAndNextPageButton()
        {
            List<string[]> expect = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {expect.Count}");
            studentsPage.ClickNextPage();
            studentsPage.ClickPreviousPage();
            List<string[]> actual = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {actual.Count}");
            CollectionAssert.AreEqual(expect, actual);
        }

        [Test]
        public void VerifyPagination_PrevAndNextPageButton_DisabledStudents()
        {
            log.Info($"Get disabed student list");
            studentsPage.ClickDisabledStudents_CheckBox();
            VerifyPagination_PrevAndNextPageButton();
        }

        [Test]
        public void VerifyPagination_ClickPrevPageButtonToEnd()
        {
            List<string[]> expectTable = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {expectTable.Count}");
            int countPage = studentsPage.GetCountOfPages();
            int indexPage = 1;
            while (indexPage <= countPage)
            {
                studentsPage.ClickNextPage();
                indexPage++;
            }
            while (indexPage >= 1)
            {
                studentsPage.ClickPreviousPage();
                indexPage--;
            }
            List<string[]> actualTable = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {actualTable.Count}");
            CollectionAssert.AreEqual(expectTable, actualTable);
        }
        [Test]
        public void VerifyPagination_ClickPrevPageButtonToEnd_DisabledStudents()
        {
            log.Info($"Get disabed student list");
            studentsPage.ClickDisabledStudents_CheckBox();
            VerifyPagination_ClickPrevPageButtonToEnd();
        }
    }
}
