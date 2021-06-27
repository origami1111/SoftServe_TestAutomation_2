using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTestInvalid : TestBase
    {
        private ChangePasswordPage changePasswordPage;

        //string currentPass = "What_123";
        //string newPass = "What_1234";



        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignIn(driver).SignInAsMentor().ClickChangePassword();
        }

        [Test]
        [TestCase("", "What_1234", "This field is required")]
        public void ChangePasswordWithInvalidCurrentPassTest(string currentEmptyPass, string newPass, string expected)
        {

            string actual = changePasswordPage
                .FillCurrentPassword(currentEmptyPass)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForCurrentPassword();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("", "What_1234", "What_123", "This field is required")]
        [TestCase("111", "What_1234", "What_123", "Password must contain at least 8 characters")]
        [TestCase("11111111", "What_1234", "What_123", "Must contain at least one uppercase, one lowercase, one number")]
        public void ChangePasswordWithInvalidNewPassTest(string newEmptyPass, string newPass, string currentPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newEmptyPass)
                .FillConfirmNewPassword(newPass)
                .VerifyErrorMassegeForNewPassword();

            Assert.AreEqual(expected, actual);
        }

        //[Test]
        //[TestCase("111", "What_1234", "What_123", "Password must contain at least 8 characters")]

        //public void ChangePasswordWithShortNewPassTest(string newShortPass, string newPass, string currentPass, string expected)
        //{
        //    string actual = changePasswordPage
        //        .FillCurrentPassword(currentPass)
        //        .FillNewPassword(newShortPass)
        //        .FillConfirmNewPassword(newPass)
        //        .VerifyErrorMassegeForNewPassword();

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ChangePasswordWithNumbersOnlyInNewPassTest()
        //{
        //    string newPassOnlyNumbers = "11111111";
        //    string expected = "Must contain at least one uppercase, one lowercase, one number";

        //    string actual = changePasswordPage
        //        .FillCurrentPassword(currentPass)
        //        .FillNewPassword(newPassOnlyNumbers)
        //        .FillConfirmNewPassword(newPass)
        //        .VerifyErrorMassegeForNewPassword();

        //    Assert.AreEqual(expected, actual);
        //}

        [Test]
        [TestCase("11111111", "What_123", "What_1234", "You should confirm your password")]
        [TestCase("", "What_123", "What_1234", "This field is required")]
        public void ChangePasswordWithInvalidConfirmPassTest(string newInvalidPass, string currentPass, string newPass, string expected)
        {
            string actual = changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newInvalidPass)
                .FillNewPassword(newPass)
                .VerifyErrorMassegeForConfirmPassword();

            Assert.AreEqual(expected, actual);
        }

        //[Test]
        //public void ChangePasswordWithEmptyConfirmPassTest()
        //{
        //    string newPassOnlyNumbers = "";
        //    string expected = "This field is required";

        //    string actual = changePasswordPage
        //        .FillCurrentPassword(currentPass)
        //        .FillNewPassword(newPass)
        //        .FillConfirmNewPassword(newPassOnlyNumbers)
        //        .FillNewPassword(newPass)
        //        .VerifyErrorMassegeForConfirmPassword();

        //    Assert.AreEqual(expected, actual);
        //}
        [TearDown]
        public void SetPostConditions()
        {
            changePasswordPage.Logout();
        }
    }
}
