using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests_VerifySearchingStudents_Valid : TestBase
    {

        private StudentsPage studentsPage;
        private static Credentials studentInfo = ReaderFileJson.ReadFileJsonCredentials(Role.Student);


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
        [TestCaseSource("StudentInfoSource")]
        public void FillSearchingField_ValidData(int id,string firstName, string lastName)
        {
            studentsPage.FillSearchingField($@"{firstName} {lastName}");
            Dictionary<int, string[]> allStudentsInfo = studentsPage.GetStudentsFromTable();
            KeyValuePair<int, string[]> ourPair = new KeyValuePair<int, string[]>(id, new string[] { firstName, lastName });
            int expected = 1;
            int actual = 0;
            foreach (var item in allStudentsInfo)
            {
                if (item.Value[0]==ourPair.Value[0]&& item.Value[1] == ourPair.Value[1])
                {
                    actual++;
                    break;
                }
            }
            Assert.AreEqual(expected, actual);

        }

        public static IEnumerable<TestCaseData> StudentInfoSource()
        {
            yield return new TestCaseData(new object[] { studentInfo.ID, studentInfo.FirstName, studentInfo.LastName});
        }

        [Test]
        [TestCase(36U)]
        public void VeriifyStudentsCount(uint stundetsCount)
        {
            uint expect = studentsPage.GetCountStudents();
            uint actual = stundetsCount;
            Assert.AreEqual(expect, actual);
        }


    }
}
