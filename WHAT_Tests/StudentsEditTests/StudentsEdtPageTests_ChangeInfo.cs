using NUnit.Framework;
using System;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsEdtPageTests_ChangeInfo : TestBase
    {
        private EditStudentDetailsPage studentsEditDetailsPage;
        private Random random = new Random();
        private int studentId;
        [SetUp]
        public void Precondition()
        {
            studentId = random.Next(4, 10);
            var account = ReaderFileJson.ReadFileJsonAccounts(Role.Admin);
            studentsEditDetailsPage = new SignInPage(driver)
                                .SignInAsAdmin(account.Email, account.Password)
                                .SidebarNavigateTo<StudentsPage>()
                                .ClickChoosedStudent(studentId)
                                .ClickEditStudentsDetaisNav()
                                .WaitStudentsEditingLoad();
        }

        [TearDown]
        public void Postcondition()
        {
            studentsEditDetailsPage.Logout();
        }

        private static IEnumerable<string[]> Source()
        {
            yield return new string[] { "FirstEditTester", "LastTester", "Student information has been edited successfully" };
        }

        [Test]
        [TestCaseSource(nameof(Source))]
        public void VerifyChangeStudentInfo_ValidDate(string firstName, string lastName, string expectAlert)
        {
            int expected = 1;
            int actual = 0;
            GenerateUser generatedUser = new GenerateUser();
            StudentsPage studentsPage = studentsEditDetailsPage.FillFirstName(firstName)
                                                     .FillLastName(lastName)
                                                     .FillEmail(generatedUser.Email)
                                                     .ClickSaveButton();
            string actualAlert = studentsPage.GetAlertText();
            StringAssert.Contains(expectAlert, actualAlert);
            KeyValuePair<int, string[]> validPair = new KeyValuePair<int, string[]>(studentId, new string[] { firstName, lastName, generatedUser.Email });
            Dictionary<int, string[]> studentTable = studentsPage.GetStudentsFromTable();
            foreach (var item in studentTable)
            {
                if (item.Key==validPair.Key)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.AreEqual(validPair.Value[0], item.Value[0]);
                        Assert.AreEqual(validPair.Value[1], item.Value[1]);
                        Assert.AreEqual(validPair.Value[2], item.Value[2]);
                    });
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
