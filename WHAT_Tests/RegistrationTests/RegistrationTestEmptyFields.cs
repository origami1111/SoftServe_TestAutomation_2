using NUnit.Framework;
using OpenQA.Selenium;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    class RegistrationTestEmptyFields : TestBase
    {
        private RegistrationPage registrationPage;

        [SetUp]
        public void SetupPage()
        {
            registrationPage = new SignInPage(driver)
                               .ClickRegistrationLink();
        }

        [Test]
        [TestCase("")]
        public void RegistrationWithEmptyFirstName(string emptyFirstName)
        {
            string expected = "This field is required";

            string actual = registrationPage
                .FillFirstName(emptyFirstName + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("")]
        public void RegistrationWithEmptyLastName(string emptyLastName)
        {
            string expected = "This field is required";

            string actual = registrationPage
                .FillLastName(emptyLastName + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("")]
        public void RegistrationWithEmptyEmail(string emptyEmail)
        {
            string expected = "This field is required";

            string actual = registrationPage
                .FillEmail(emptyEmail + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("")]
        public void RegistrationWithEmptyPassword(string emptyPassword)
        {
            string expected = "This field is required";

            string actual = registrationPage
                .FillPassword(emptyPassword + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("")]
        public void RegistrationWithEmptyConfirmPassword(string emptyConfirmPassword)
        {
            string expected = "This field is required";

            string actual = registrationPage
                .FillConfirmPassword(emptyConfirmPassword + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

    }
}
