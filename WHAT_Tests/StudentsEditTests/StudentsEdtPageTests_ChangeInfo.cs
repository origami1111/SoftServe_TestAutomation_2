using NUnit.Framework;
using System;
using System.Collections.Generic;
using WHAT_API;
using WHAT_API.Entities;
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
        [Test]
        [TestCase("FirstEditTester", "LastTester", "qqqqqqqwew@gmail.com", "Student information has been edited successfully")]
        public void VerifyChangeStudentInfo_ValidDate(string firstName, string lastName, string email, string expectAlert)
        {
            var actualAlert = studentsEditDetailsPage.FillFirstName(firstName)
                                                     .FillLastName(lastName)
                                                     .FillEmail(email)
                                                     .ClickSaveButton()
                                                     .GetAlertText();
            Assert.AreEqual(expectAlert, actualAlert);
        }
    }
}
