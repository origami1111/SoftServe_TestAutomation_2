using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
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
        }

        [TearDown]
        public void Postcondition()
        {
            studentsPage.Logout();
        }

        [Test]
        public void VerifyPagination_PrevAndNextPageButton()
        {
            Dictionary<int, string[]> expect = studentsPage.GetStudentsFromTable();
            studentsPage.ClickNextPage();
            studentsPage.ClickPreviousPage();
            Dictionary<int, string[]> actual = studentsPage.GetStudentsFromTable();
            CollectionAssert.AreEqual(expect, actual);
        }

        [Test]
        public void VerifyPagination_PrevAndNextPageButton_DisabledStudents()
        {
            studentsPage.ClickDisabledStudents_CheckBox();
            VerifyPagination_PrevAndNextPageButton();
        }

        [Test]
        public void VerifyPagination_ClickPrevPageButtonToEnd()
        {
            Dictionary<int, string[]> expectTable = studentsPage.GetStudentsFromTable();
            Dictionary<int, string[]> expectCurrPage = new Dictionary<int, string[]>();
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
            Dictionary<int, string[]> actualTable = studentsPage.GetStudentsFromTable();
            CollectionAssert.AreEqual(expectTable, actualTable);
        }
        [Test]
        public void VerifyPagination_ClickPrevPageButtonToEnd_DisabledStudents()
        {
            studentsPage.ClickDisabledStudents_CheckBox();
            VerifyPagination_ClickPrevPageButtonToEnd();
        }
    }
}
