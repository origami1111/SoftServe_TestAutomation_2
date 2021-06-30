using NUnit.Framework;
using WHAT_PageObject;


namespace WHAT_Tests
{
    [TestFixture]
    public class SecretariesPaginationTests : TestBase
    {
        private SecretariesPage secretariesPage;

        [SetUp]
        public void Setup()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            secretariesPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>();
        }

        [TearDown]
        public void Down()
        {
            secretariesPage.Logout();
        }

        [TestCase(ShowedUsers.ten)]
        [TestCase(ShowedUsers.fifty)]
        [TestCase(ShowedUsers.oneHundred)]
        [Test]
        public void VerifyFirstPageCount(ShowedUsers usersOnPage)
        {
            int expected;
            secretariesPage.SelectUsersAtPage(usersOnPage);
            int actual = secretariesPage.GetShowedUsersAmount();
            if (secretariesPage.GetLastUserIndex() >= secretariesPage.GetUsersAtPage())
            {
                expected = secretariesPage.GetUsersAtPage();
            }
            else
            {
                expected = secretariesPage.GetLastUserIndex();
            }
            Assert.AreEqual(expected, actual);
        }

        [TestCase(ShowedUsers.ten)]
        [TestCase(ShowedUsers.fifty)]
        [TestCase(ShowedUsers.oneHundred)]
        [Test]
        public void VerifyLastPageCount(ShowedUsers usersOnPage)
        {           
            secretariesPage.SelectUsersAtPage(usersOnPage);
            int expected = secretariesPage.GetLastUserIndex() % secretariesPage.GetUsersAtPage();
            int actual = secretariesPage.GetShowedUsersAmount();
            Assert.AreEqual(expected, actual);
        }

        [TestCase(ShowedUsers.ten)]
        [TestCase(ShowedUsers.fifty)]
        [TestCase(ShowedUsers.oneHundred)]
        [Test]
        public void VerifyMidlePageCount(ShowedUsers usersOnPage)
        {
            secretariesPage.SelectUsersAtPage(usersOnPage);
            if(secretariesPage.GetPagesAmount()>2)
            {
                int expected = secretariesPage.GetUsersAtPage();
                int actual = secretariesPage.PrevPage().GetShowedUsersAmount();
                Assert.AreEqual(expected, actual);
            }
            else
            {

            }          
        }

    }
}
