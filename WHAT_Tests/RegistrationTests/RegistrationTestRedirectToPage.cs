using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    class RegistrationTestRedirectToPage : TestBase
    {
        private RegistrationPage registrationPage;

        [SetUp]
        public void SetupPage()
        {
            registrationPage = new SignInPage(driver)
                .ClickRegistrationLink();
        }

        [Test]
        public void RedirectToSignInPage()
        {
            string expected = ReaderUrlsJSON.GetUrlByName("SigninPage");

            registrationPage.ClickLogInLink();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

    }
}
