using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [AllureNUnit]
    [TestFixture]
    public class StudentsEdtPageTests_ChangeInfo : TestBase
    {
        private EditStudentDetailsPage studentsEditDetailsPage;
        private StudentsPage studentsPage;
        private Random random = new Random();
        public StudentsEdtPageTests_ChangeInfo()
        {
            log = LogManager.GetLogger($"Students Details page/{nameof(StudentsEdtPageTests_ChangeInfo)}");
        }

        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>();
            int studentId = random.Next(3, studentsPage.GetCountStudents());
            studentsEditDetailsPage=studentsPage.ClickChoosedStudent(studentId)
                                .ClickEditStudentsDetaisNav()
                                .WaitStudentsEditingLoad();
            log.Info($"Go to {driver.Url}");
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
            log.Info($"Generate user random email: {generatedUser.Email}");
            studentsPage = studentsEditDetailsPage.FillFirstName(firstName)
                                                     .FillLastName(lastName)
                                                     .FillEmail(generatedUser.Email)
                                                     .ClickSaveButton();
            string actualAlert = studentsPage.GetAlertText();
            log.Info($"Get allert text: {actualAlert}");
            StringAssert.Contains(expectAlert, actualAlert);
            string[] validPair =  new string[] { firstName, lastName, generatedUser.Email };
            List< string[]> studentTable = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {studentTable.Count}");
            foreach (var item in studentTable)
            {
                if (item[(int)RowOfElement.Email-1]== validPair[(int)RowOfElement.Email - 1])
                {
                    Assert.AreEqual(validPair[(int)RowOfElement.FirstName - 1], item[(int)RowOfElement.FirstName - 1]);
                    Assert.AreEqual(validPair[(int)RowOfElement.LastName - 1], item[(int)RowOfElement.LastName - 1]);
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
