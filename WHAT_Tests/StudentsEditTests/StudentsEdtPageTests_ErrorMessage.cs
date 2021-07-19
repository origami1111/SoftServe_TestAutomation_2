using NUnit.Framework;
using System;
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
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsEditDetailsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password)
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

        [Test]
        [TestCase("a", "Too short")]
        [TestCase(" ", "Too short")]
        [TestCase("", "This field is required")]
        [TestCase("Detail name with more than 50 characters is too long", "Too long")]
        [TestCase(" beforeSpace", "Invalid first name")]
        [TestCase("More than one space    between words", "Invalid first name")]
        [TestCase("SpaceAfterFirst name ", "Invalid first name")]
        [TestCase("Name*/!&?", "Invalid first name")]
        public void VerifyFillingDetailsField_FirstName_InvalidDate(string invalidData, string expectedErrorMessage)
        {
            var actualErrorMessage = studentsEditDetailsPage.FillFirstName(invalidData)
                                                           .GetErrorMessageFirstName();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }
        [Test]
        [TestCase("z", "Too short")]
        [TestCase(" ", "Too short")]
        [TestCase("", "This field is required")]
        [TestCase("Detail name with more than 50 characters is too long", "Too long")]
        [TestCase(" beforeSpace", "Invalid last name")]
        [TestCase("More than one space   between words", "Invalid last name")]
        [TestCase("SpaceAfterLast name ", "Invalid last name")]
        [TestCase("*/!&?Name", "Invalid last name")]
        public void VerifyFillingDetailsField_LastName_InvalidDate(string invalidData, string expectedErrorMessage)
        {
            var actualErrorMessage = studentsEditDetailsPage.FillLastName(invalidData)
                                                           .GetErrorMessageLastName();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }

        [Test]
        [TestCase("b", "Invalid email address")]
        [TestCase(" ", "This field is required")]
        [TestCase("", "This field is required")]
        [TestCase("    beforeword@gmail.com", "Invalid email address")]
        [TestCase("afterword@gmail.com     ", "Invalid email address")]
        [TestCase("      inthemiddleword@gmail.com     ", "Invalid email address")]
        [TestCase("middle space@gmail.com", "Invalid email address")]
        [TestCase("middle space@gmail.com", "Invalid email address")]
        [TestCase("@gmail.com", "Invalid email address")]
        [TestCase("1@gmail.com", "Invalid email address")]
        public void VerifyFillingDetailsField_Email_InvalidDate(string invalidData, string expectedErrorMessage)
        {
            var actualErrorMessage = studentsEditDetailsPage.FillEmail(invalidData)
                                                           .GetErrorMessageEmail();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }
    }
}
