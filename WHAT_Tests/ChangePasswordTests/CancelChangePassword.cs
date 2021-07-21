using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class CancelChangePassword : TestBase
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
        [TestCase("What_123", "What_1234")]
        public void CancelChangePasswordTest(string currentPass, string newPass)
        {
            changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPass)
                .ClickCancelButton();

            changePasswordPage.Logout();

            changePasswordPage = new SignInPage(driver)
                            .SignInAsMentor(account.Email, account.Password)
                            .ClickChangePassword();
        }

        [TearDown]
        public void SetPostConditions()
        {
            changePasswordPage.Logout();
        }
    }
}
