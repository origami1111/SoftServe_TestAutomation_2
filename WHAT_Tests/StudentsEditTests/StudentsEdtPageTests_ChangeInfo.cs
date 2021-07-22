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
        private Random random = new Random();
        private int studentId;

        public StudentsEdtPageTests_ChangeInfo()
        {
            log = LogManager.GetLogger($"Students Details page/{nameof(StudentsEdtPageTests_ChangeInfo)}");
        }

        [SetUp]
        public void Precondition()
        {
            studentId = random.Next(4, 10);
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsEditDetailsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>()
                                .ClickChoosedStudent(studentId)
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
            StudentsPage studentsPage = studentsEditDetailsPage.FillFirstName(firstName)
                                                     .FillLastName(lastName)
                                                     .FillEmail(generatedUser.Email)
                                                     .ClickSaveButton();
            string actualAlert = studentsPage.GetAlertText();
            log.Info($"Get allert text: {actualAlert}");
            StringAssert.Contains(expectAlert, actualAlert);
            KeyValuePair<int, string[]> validPair = new KeyValuePair<int, string[]>(studentId, new string[] { firstName, lastName, generatedUser.Email });
            List< string[]> studentTable = studentsPage.GetStudentsFromTable();
            log.Info($"Get student table, count: {studentTable.Count}");
            foreach (var item in studentTable)
            {
                    Assert.Multiple(() =>
                    {
                        Assert.AreEqual(validPair.Value[0], item[0]);
                        Assert.AreEqual(validPair.Value[1], item[1]);
                        Assert.AreEqual(validPair.Value[2], item[2]);
                    });
                    actual++;
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
