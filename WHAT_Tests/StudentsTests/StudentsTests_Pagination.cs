using NUnit.Framework;
using System.Threading;
using WHAT_PageObject;
using System.Collections.Generic;
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
        public void VerifyPagination_ClickPrevPageButtonToEnd()
        {
            Dictionary<int, string[]> expect = studentsPage.GetStudentsFromTable();
            uint countPage = studentsPage.GetCountOfPages();
            uint index = 1;
            while (index <= countPage)
            {
                studentsPage.ClickNextPage();
                index++;
            }
            while (index >= 1)
            {
                studentsPage.ClickPreviousPage();
                index--;
            }
            Dictionary<int, string[]> actual = studentsPage.GetStudentsFromTable();
            CollectionAssert.AreEqual(expect, actual);
        }
    }
}
