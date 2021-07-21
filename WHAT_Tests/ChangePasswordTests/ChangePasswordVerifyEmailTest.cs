using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordVerifyEmailTest : TestBase
    {
        private ChangePasswordPage changePasswordPage;
        Account account = ReaderFileJson.ReadFileJsonAccounts(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            changePasswordPage = new SignInPage(driver)
                               .SignInAsMentor(account.Email, account.Password)
                               .ClickChangePassword();
        }
        [Test]
        public void VerifyEmailTest()
        {
            string expected = account.Email;
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

