using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
   public class ChangePasswordVerifyEmailTest : TestBase
    {
    private ChangePasswordPage changePasswordPage;

    string currentPass = "What_123";
    string email = "mentor@gmail.com";

    [SetUp]
    public void SetupPage()
    {
        changePasswordPage = new SignInPage(driver)
            .SignInAsMentor(email, currentPass).ClickChangePassword();
    }

    [Test]
    public void VerifyEmailTest()
    {
        string expected = email;
        string actual = changePasswordPage.VerifyCurrentEmail();
            
        Assert.AreEqual(expected, actual);
    }

    [TearDown]
    public void SetPostConditions()
    {
            changePasswordPage.Logout();
    }
}
}
