using NUnit.Framework;
using OpenQA.Selenium;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    class RegistrationTestInvalidData : TestBase
    {
        private RegistrationPage registrationPage;

        [SetUp]
        public void SetupPage()
        {
            registrationPage = new SignInPage(driver)
                .ClickRegistrationLink();
        }

        [Test]
        [TestCase("a", "Too short")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Too long")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Too long")]
        [TestCase("Name==", "Invalid first name")]
        [TestCase("Name111", "Invalid first name")]
        public void RegistrationWithInvalidDataFirstNameTest(string invalidData, string expected)
        {
            string actual = registrationPage
                .FillFirstName(invalidData + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("a", "Too short")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Too long")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Too long")]
        [TestCase("LastName==", "Invalid last name")]
        [TestCase("LastName111", "Invalid last name")]
        public void RegistrationWithInvalidDataLastNameTest(string invalidData, string expected)
        {
            string actual = registrationPage
                .FillLastName(invalidData + Keys.Enter)
                .GetErrorMessageLastName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("email.com", "Invalid email address")]
        [TestCase("email@com", "Invalid email address")]
        public void RegistrationWithInvalidDataEmailTest(string invalidData, string expected)
        {
            string actual = registrationPage
                .FillEmail(invalidData + Keys.Enter)
                .GetErrorMessageEmail();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("q", "Password must contain at least 8 characters")]
        [TestCase("qwerty1", "Password must contain at least 8 characters")]
        [TestCase("qwertyqwerty", "Password must contain at least one uppercase, one lowercase, one number and one special symbol")]
        [TestCase("qwertyqwerty1", "Password must contain at least one uppercase, one lowercase, one number and one special symbol")]
        public void RegistrationWithInvalidDataPasswordTest(string invalidData, string expected)
        {
            string actual = registrationPage
                .FillPassword(invalidData + Keys.Enter)
                .GetErrorMessagePassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("Qwerty_123", "ytrewq", "You should confirm your password")]
        public void RegistrationWithInvalidDataPasswordConfirmTest(string password, string confirmPassword, string expected)
        {
            string actual = registrationPage
                .FillPassword(password)
                .FillConfirmPassword(confirmPassword + Keys.Enter)
                .GetErrorMessageConfirmPassword();

            Assert.AreEqual(expected, actual);
        }

    }
}
