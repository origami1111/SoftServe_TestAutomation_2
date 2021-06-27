using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordVerifyEmailTest : TestBase
    {
        private ChangePasswordPage changePasswordPage;

        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignIn(driver).SignInAsMentor().ClickChangePassword();
        }

        [Test]
        [TestCase("mentor@gmail.com")]
        public void VerifyEmailTest(string expected)
        {
            
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
