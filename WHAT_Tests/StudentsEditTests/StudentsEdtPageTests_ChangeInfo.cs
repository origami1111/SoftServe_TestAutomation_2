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

        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsEditDetailsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
                                .SidebarNavigateTo<StudentsPage>()
                                .ClickChoosedStudent(random.Next(4, 10))
                                .ClickEditStudentsDetaisNav()
                                .WaitStudentsEditingLoad();
        }

        public StudentsEdtPageTests_ChangeInfo()
        { 
            
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
            GenerateUser generatedUser = new GenerateUser();
            var actualAlert = studentsEditDetailsPage.FillFirstName(firstName)
                                                     .FillLastName(lastName)
                                                     .FillEmail(generatedUser.Email)
                                                     .ClickSaveButton()
                                                     .GetPopUpText();
            Assert.AreEqual(expectAlert, actualAlert);
        }
    }
}
