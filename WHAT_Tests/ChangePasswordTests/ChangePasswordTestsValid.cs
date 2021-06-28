using NUnit.Framework;
using WHAT_PageObject;
namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTestsValid : TestBase
    {
        
        private ChangePasswordPage changePasswordPage;

        string currentPass = "What_123";
        string newPass = "What_1234";
        string email = "mentor@gmail.com";


        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignInPage(driver)
                .SignInAsMentor(email, currentPass).ClickChangePassword();
        }

        [Test]
        public void ChangePasswordWithValidDataTest()
        {
            string expected = "×\r\nClose alert\r\nThe password has been successfully changed";
            string actual= changePasswordPage
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
            changePasswordPage
                .ClickChangePassword()
                .FillCurrentPassword(newPass)
                .FillNewPassword(currentPass)
                .FillConfirmNewPassword(currentPass)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu();
        }

    }
}
