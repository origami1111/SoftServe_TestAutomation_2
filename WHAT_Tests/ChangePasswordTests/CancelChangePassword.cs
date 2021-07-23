using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Tests.ChangePasswordTests;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class CancelChangePassword : TestBase
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

        [TestCaseSource(typeof(TestCasesChangePassword), nameof(TestCasesChangePassword.CancelChangePassword))]
        public void CancelChangePasswordTest(string currentPass, string newPass)
        {
            changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPass)
                .ClickCancelButton();

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
