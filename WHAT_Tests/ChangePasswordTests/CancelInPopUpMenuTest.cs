using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CancelInPopUpMenuTest : TestBase
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

        [Test]
        [TestCase("What_123", "What_1234")]
        public void CancelChangePasswordInPopUpMenuTest(string currentPass, string newPass)
        {
            changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPass)
                .ClickSaveButton()
                .ClickCancelButtonInPopUpMenu();

            changePasswordPage.Logout();

            changePasswordPage = new SignInPage(driver)
                            .SignInAsMentor(credentials.Email, credentials.Password)
                            .ClickChangePassword();
        }

        [TearDown]
        public void SetPostConditions()
        {
            changePasswordPage.Logout();
        }
    }
}
