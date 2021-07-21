using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTestsValid : TestBase
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
        [TestCase("What_123", "What_1234", "×\r\nClose alert\r\nThe password has been successfully changed")]
        public void ChangePasswordWithValidDataTest(string currentPass, string newPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPass)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu()
                .VerifySuccesMessage();

            Assert.AreEqual(expected, actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            string postNewPass = "What_123";
            string postCurrPass = "What_1234";
            changePasswordPage
                .ClickChangePassword()
                .FillCurrentPassword(postCurrPass)
                .FillNewPassword(postNewPass)
                .FillConfirmNewPassword(postNewPass)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu();
        }
    }
}
