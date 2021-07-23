using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [AllureNUnit]
    [TestFixture]
    class SignInTestWithInvalidData : TestBase
    {
        private SignInPage signInPage;

        [SetUp]
        public void SetupPage()
        {
            signInPage = new SignInPage(driver);
        }

        [Test]
        [TestCase("email", "password")]
        public void SignInAsWithInvalidData(string email, string password)
        {
            signInPage
                .FillEmail(email)
                .FillPassword(password)
                .ClickSignInButton();

            Assert.AreEqual("An error occurred", signInPage.GetErrorText());
        }

    }
}
