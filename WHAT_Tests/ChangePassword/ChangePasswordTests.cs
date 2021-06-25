using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTests : TestBase
    {
        
        //private ChangePasswordPage changePasswordPage;

        //string currentPass = "What_123";
        //string newPass = "What_1234";
        //string email = "mentor@gmail.com";


        //[SetUp]
        //public void SetupPage()
        //{
        //    changePasswordPage = new SignInPage(driver)
        //        .SignInAsMentor(email, currentPass).ClickChangePassword();
        //}

        //[Test]
        //public void ChangePasswordTest()
        //{
           
        //    changePasswordPage
        //        .FillCurrentPassword(currentPass)
        //        .FillNewPassword(newPass)
        //        .FillConfirmNewPassword(newPass)
        //        .ClickSaveButton()
        //        .ClickSaveInPopUpMenu();

        //}

        //[TearDown]
        //public void SetPostConditions()
        //{
        //    changePasswordPage
        //        .ClickChangePassword()
        //        .FillCurrentPassword(newPass)
        //        .FillNewPassword(currentPass)
        //        .FillConfirmNewPassword(currentPass)
        //        .ClickSaveButton()
        //        .ClickSaveInPopUpMenu();
               
        //}

    }
}
