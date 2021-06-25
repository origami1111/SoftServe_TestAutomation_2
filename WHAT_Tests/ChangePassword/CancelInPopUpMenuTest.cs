using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WHAT_PageObject;

namespace WHAT_Tests
{
   [TestFixture]
   public class CancelInPopUpMenuTest : TestBase
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
        public void CancelChangePasswordInPopUpMenuTest()
        {
            changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPass)
                .ClickSaveButton()
                .ClickCancelButtonInPopUpMenu();

            changePasswordPage.Logout();

            changePasswordPage = new SignInPage(driver)
                .SignInAsMentor(email, currentPass).ClickChangePassword();
        }

        [TearDown]
        public void SetPostConditions()
        {
            changePasswordPage.Logout();
        }
    }
}
