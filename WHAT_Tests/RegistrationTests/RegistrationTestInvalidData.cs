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

        #region FirstNameField
        [Test]
        [TestCase("a")]
        public void RegistrationWithTooShortFirstNameTest(string tooShortFirstName)
        {
            string expected = "Too short";

            string actual = registrationPage
                .FillFirstName(tooShortFirstName + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void RegistrationWithTooLongFirstNameTest(string tooLongFirstName)
        {
            string expected = "Too long";

            string actual = registrationPage
                .FillFirstName(tooLongFirstName + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("==")]
        public void RegistrationWithSpecialSymbolsFirstNameTest(string specialSymbolsFirstName)
        {
            string expected = "Invalid first name";

            string actual = registrationPage
                .FillFirstName(specialSymbolsFirstName + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("111")]
        public void RegistrationWithNumbersFirstNameTest(string numbersFirstName)
        {
            string expected = "Invalid first name";

            string actual = registrationPage
                .FillFirstName(numbersFirstName + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region LastNameField
        [Test]
        [TestCase("a")]
        public void RegistrationWithTooShortLastNameTest(string tooShortLastName)
        {
            string expected = "Too short";

            string actual = registrationPage
                .FillLastName(tooShortLastName + Keys.Enter)
                .GetErrorMessageLastName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void RegistrationWithTooLongLastNameTest(string tooLongLastName)
        {
            string expected = "Too long";

            string actual = registrationPage
                .FillLastName(tooLongLastName + Keys.Enter)
                .GetErrorMessageLastName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("==")]
        public void RegistrationWithSpecialSymbolsLastNameTest(string specialSymbolsLastName)
        {
            string expected = "Invalid last name";

            string actual = registrationPage
                .FillLastName(specialSymbolsLastName + Keys.Enter)
                .GetErrorMessageLastName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("111")]
        public void RegistrationWithNumbersLastNameTest(string numbersLastName)
        {
            string expected = "Invalid last name";

            string actual = registrationPage
                .FillLastName(numbersLastName + Keys.Enter)
                .GetErrorMessageLastName();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Email
        [Test]
        [TestCase("email.com")]
        public void RegistrationWithoutAtInEmailTest(string withoutAt)
        {
            string expected = "Invalid email address";

            string actual = registrationPage
                .FillEmail(withoutAt + Keys.Enter)
                .GetErrorMessageEmail();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("email@com")]
        public void RegistrationWithoutDotInEmailTest(string withoutDot)
        {
            string expected = "Invalid email address";

            string actual = registrationPage
                .FillEmail(withoutDot + Keys.Enter)
                .GetErrorMessageEmail();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region PasswordField
        [Test]
        [TestCase("qwerty")]
        public void RegistrationWithShortPasswordTest(string shortPassword)
        {
            string expected = "Password must contain at least 8 characters";

            string actual = registrationPage
                .FillPassword(shortPassword + Keys.Enter)
                .GetErrorMessagePassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("qwertyqwerty")]
        public void RegistrationWithOnlyLowerCaseLettersPasswordTest(string withLowerCaseLetters)
        {
            string expected = "Must contain at least one uppercase, one lowercase, one number";

            string actual = registrationPage
                .FillPassword(withLowerCaseLetters + Keys.Enter)
                .GetErrorMessagePassword();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region ConfirmPasswordField
        [Test]
        [TestCase("Qwerty_123", "ytrewq")]
        public void RegistrationWithMismatchedPasswordConfirmTest(string password, string confirmPassword)
        {
            string expected = "You should confirm your password";

            string actual = registrationPage
                .FillPassword(password)
                .FillConfirmPassword(confirmPassword + Keys.Enter)
                .GetErrorMessageConfirmPassword();

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
