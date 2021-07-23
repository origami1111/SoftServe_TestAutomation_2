using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class ChangePasswordVerifyEmailTest : TestBase
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
        public void VerifyEmailTest()
        {
            string expected = credentials.Email;
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

