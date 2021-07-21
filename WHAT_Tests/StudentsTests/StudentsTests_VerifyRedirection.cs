using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;
using System.Linq;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests_VerifyRedirection : TestBase
    {
        private StudentsPage studentsPage;

        [SetUp]
        public void Precondition()
        {
            var account = ReaderFileJson.ReadFileJsonAccounts(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(account.Email, account.Password)
                                .SidebarNavigateTo<StudentsPage>();
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
            Dictionary<int, string[]> allStudentsInfo = studentsPage.GetStudentsFromTable();
            var expecteStudentInfo = allStudentsInfo.Values.ElementAt(studentNum-1);
            StudentDetailsPage studentsEdit =studentsPage.ClickChoosedStudent(studentNum);
            string[] actualStudenInfo = studentsEdit.GetTexFromAllField();
            CollectionAssert.AreEqual(expecteStudentInfo, actualStudenInfo);
        }
    }
}
