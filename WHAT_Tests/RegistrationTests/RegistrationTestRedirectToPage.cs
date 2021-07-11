using NUnit.Framework;
using WHAT_Utilities;
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
            string expected = ReaderUrlsJSON.GetUrlByName("SigninPage", LinksPath);

            registrationPage.ClickLogInLink();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

    }
}
