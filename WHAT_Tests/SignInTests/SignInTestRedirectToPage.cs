using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    class SignInTestRedirectToPage : TestBase
    {
        private SignInPage signInPage;

        [SetUp]
        public void SetupPage()
        {
            signInPage = new SignInPage(driver);
        }

        [Test]
        public void RedirectToRegistrationPage()
        {
            string expected = ReaderUrlsJSON.GetUrlByName("RegistrationPage");

            signInPage.ClickRegistrationLink();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }
    }
}
