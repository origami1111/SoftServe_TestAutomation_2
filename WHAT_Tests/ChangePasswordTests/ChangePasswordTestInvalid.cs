using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Tests.ChangePasswordTests;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTestInvalid : TestBase
    {
        private ChangePasswordPage changePasswordPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignInPage(driver)
                .SignInAsMentor(credentials.Email, credentials.Password)
                .ClickChangePassword();
        }

        [TestCaseSource(typeof(TestCasesChangePassword), nameof(TestCasesChangePassword.InvalidCurrentPass))]
        public void ChangePasswordWithInvalidCurrentPassTest(string invalidCurrentPass, string newPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(invalidCurrentPass)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForCurrentPassword();

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(TestCasesChangePassword), nameof(TestCasesChangePassword.InvalidNewPass))]
        public void ChangePasswordWithInvalidNewPassTest(string invalidNewPass, string newPass, string currentPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(invalidNewPass)
                .FillConfirmNewPassword(newPass)
                .VerifyErrorMassegeForNewPassword();

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(TestCasesChangePassword), nameof(TestCasesChangePassword.InvalidConfirmPass))]
        public void ChangePasswordWithInvalidConfirmPassTest(string invalidConfirmPass, string currentPass, string newPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(invalidConfirmPass)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForConfirmPassword();

            Assert.AreEqual(expected, actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            changePasswordPage.Logout();
        }
    }
}
