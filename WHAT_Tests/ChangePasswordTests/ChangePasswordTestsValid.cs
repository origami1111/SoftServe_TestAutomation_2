using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class ChangePasswordTestsValid : TestBase
    {
        private ChangePasswordPage changePasswordPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);
        private string newPassword = "What_1234";

        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignInPage(driver)
                .SignInAsMentor(credentials.Email, credentials.Password)
                .ClickChangePassword();
        }

        [Test]
        public void ChangePasswordWithValidDataTest()
        {
            string expected = "The password has been successfully changed";

            string actual = changePasswordPage
                .FillCurrentPassword(credentials.Password)
                .FillNewPassword(newPassword)
                .FillConfirmNewPassword(newPassword)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu()
                .VerifySuccesMessage();

            StringAssert.Contains(expected,actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            changePasswordPage
                .ClickChangePassword()
                .FillCurrentPassword(newPassword)
                .FillNewPassword(credentials.Password)
                .FillConfirmNewPassword(credentials.Password)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu();
        }
    }
}
