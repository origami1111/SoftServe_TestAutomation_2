using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTestInvalid : TestBase
    {
        private ChangePasswordPage changePasswordPage;
        Account account = ReaderFileJson.ReadFileJsonAccounts(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignInPage(driver)
                           .SignInAsMentor(account.Email, account.Password)
                           .ClickChangePassword();
        }

        [Test]
        [TestCase("", "What_1234", "This field is required")]
        public void ChangePasswordWithInvalidCurrentPassTest(string invalidCurrentPass, string newPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(invalidCurrentPass)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForCurrentPassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("", "What_1234", "What_123", "This field is required")]
        [TestCase("111", "What_1234", "What_123", "Password must contain at least 8 characters")]
        [TestCase("11111111", "What_1234", "What_123", "Must contain at least one uppercase, one lowercase, one number")]
        public void ChangePasswordWithInvalidNewPassTest(string invalidNewPass, string newPass, string currentPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(invalidNewPass)
                .FillConfirmNewPassword(newPass)
                .VerifyErrorMassegeForNewPassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("11111111", "What_123", "What_1234", "You should confirm your password")]
        [TestCase("", "What_123", "What_1234", "This field is required")]
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
