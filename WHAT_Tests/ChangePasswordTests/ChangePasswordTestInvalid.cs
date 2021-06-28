using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
     public class ChangePasswordTestInvalid : TestBase
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
        public void ChangePasswordWithEmptyCurrentPassTest()
        {
            string currentEmptyPass = "";
            string expected = "This field is required";

            string actual= changePasswordPage
                .FillCurrentPassword(currentEmptyPass)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForCurrentPassword();
           
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangePasswordWithEmptyNewPassTest()
        {
            string newEmptyPass = "";
            string expected = "This field is required";

            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newEmptyPass)
                .FillConfirmNewPassword(newPass)
                .VerifyErrorMassegeForNewPassword();
                
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangePasswordWithShortNewPassTest()
        {
            string newShortPass = "111";
            string expected = "Password must contain at least 8 characters";

            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newShortPass)
                .FillConfirmNewPassword(newPass)
                .VerifyErrorMassegeForNewPassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangePasswordWithNumbersOnlyInNewPassTest()
        {
            string newPassOnlyNumbers = "11111111";
            string expected = "Must contain at least one uppercase, one lowercase, one number";

            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPassOnlyNumbers)
                .FillConfirmNewPassword(newPass)
                .VerifyErrorMassegeForNewPassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangePasswordWithNumbersOnlyInConfirmPassTest()
        {
            string newPassOnlyNumbers = "11111111";
            string expected = "You should confirm your password";

            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPassOnlyNumbers)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForConfirmPassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangePasswordWithEmptyConfirmPassTest()
        {
            string newPassOnlyNumbers = "";
            string expected = "This field is required";

            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPassOnlyNumbers)
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
