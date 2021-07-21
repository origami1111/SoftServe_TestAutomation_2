using NUnit.Framework;
using System;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsEdtPageTests_ErrorMessage: TestBase
    {
        private EditStudentDetailsPage studentsEditDetailsPage;
        private Random random = new Random();

        [SetUp]
        public void Precondition()
        {
            var account = ReaderFileJson.ReadFileJsonAccounts(Role.Admin);
            studentsEditDetailsPage = new SignInPage(driver)
                                .SignInAsAdmin(account.Email, account.Password)
                                .SidebarNavigateTo<StudentsPage>()
                                .ClickChoosedStudent(random.Next(1, 10))
                                .ClickEditStudentsDetaisNav()
                                .WaitStudentsEditingLoad();
        }

        [TearDown]
        public void Postcondition()
        {
            studentsEditDetailsPage.Logout();
        }
        
        [Test, TestCaseSource(nameof(InvalidFirstNameSource))]
        public void VerifyFillingDetailsField_FirstName_InvalidDate(string invalidData, string expectedErrorMessage)
        {
            var actualErrorMessage = studentsEditDetailsPage.FillFirstName(invalidData)
                                                           .GetErrorMessageFirstName();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }

        [Test, TestCaseSource(nameof(InvalidLastNameSource))]
        public void VerifyFillingDetailsField_LastName_InvalidDate(string invalidData, string expectedErrorMessage)
        {
            var actualErrorMessage = studentsEditDetailsPage.FillLastName(invalidData)
                                                           .GetErrorMessageLastName();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }

        [Test, TestCaseSource(nameof(InvalidEmailSource))]
        public void VerifyFillingDetailsField_Email_InvalidDate(string invalidData, string expectedErrorMessage)
        {
            var actualErrorMessage = studentsEditDetailsPage.FillEmail(invalidData)
                                                           .GetErrorMessageEmail();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }

        private static IEnumerable<object[]> InvalidFirstNameSource()
        {
            yield return new object[] { "a", "Too short" };
            yield return new object[] { " ", "Too short" };
            yield return new object[] { "", "This field is required" };
            yield return new object[] { "aaaaaaaaaaaaaaaaaaaaaayuiopqwertyuoiasdfghjkoplmbі", "Too long" };
            yield return new object[] { "_hyphen", "Invalid first name" };
            yield return new object[] { "hyphen_", "Invalid first name" };
            yield return new object[] { "SpaceAfterFirst name ", "Invalid first name" };
            yield return new object[] { " beforeSpace", "Invalid first name" };
            yield return new object[] { "*/Name", "Invalid first name" };
            yield return new object[] { "Na*me", "Invalid first name" };
            yield return new object[] { "Name'", "Invalid first name" };
        }
        private static IEnumerable<object[]> InvalidLastNameSource()
        {
            yield return new object[] { "b", "Too short" };
            yield return new object[] { " ", "Too short" };
            yield return new object[] { "", "This field is required" };
            yield return new object[] { "aaaaaaaaaaaaaaaaaaaaaayuiopqwertyuoiasdfghjkoplmbі", "Too long" };
            yield return new object[] { "_hyphen", "Invalid last name" };
            yield return new object[] { "hyphen_", "Invalid last name" };
            yield return new object[] { "SpaceAfterFirst name ", "Invalid last name" };
            yield return new object[] { " beforeSpace", "Invalid last name" };
            yield return new object[] { "*/Name", "Invalid last name" };
            yield return new object[] { "Na*me", "Invalid last name" };
            yield return new object[] { "Name@", "Invalid last name" };
        }
        private static IEnumerable<object[]> InvalidEmailSource()
        {
            yield return new object[] { "c", "Too short" };
            yield return new object[] { " ", "Too short" };
            yield return new object[] { "", "This field is required" };
            yield return new object[] { "    beforeword@gmail.com", "Invalid email address" };
            yield return new object[] { "afterword@gmail.com     ", "Invalid email address" };
            yield return new object[] { "      inthemiddleword@gmail.com     ", "Invalid email address" };
            yield return new object[] { "middle space@gmail.com", "Invalid email address" };
            yield return new object[] { "@gmail.com", "Invalid email address" };
            yield return new object[] { "1@gmail.com", "Invalid email address" };
        }
    }
}
