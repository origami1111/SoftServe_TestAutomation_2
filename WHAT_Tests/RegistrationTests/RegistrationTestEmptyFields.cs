using NUnit.Framework;
using OpenQA.Selenium;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    class RegistrationTestEmptyFields : TestBase
    {
        private RegistrationPage registrationPage;
        private string expected = "This field is required";
        private string emptyField = "";

        [SetUp]
        public void SetupPage()
        {
            registrationPage = new SignInPage(driver)
                .ClickRegistrationLink();
        }

        [Test]
        public void RegistrationWithEmptyFirstName()
        {
            string actual = registrationPage
                .FillFirstName(emptyField + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegistrationWithEmptyLastName()
        {
            string actual = registrationPage
                .FillLastName(emptyField + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegistrationWithEmptyEmail()
        {
            string actual = registrationPage
                .FillEmail(emptyField + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegistrationWithEmptyPassword()
        {
            string actual = registrationPage
                .FillPassword(emptyField + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegistrationWithEmptyConfirmPassword()
        {
            string actual = registrationPage
                .FillConfirmPassword(emptyField + Keys.Enter)
                .GetErrorMessageFirstName();

            Assert.AreEqual(expected, actual);
        }

    }
}
