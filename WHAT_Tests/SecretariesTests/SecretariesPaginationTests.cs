using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

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
            int lastUserIndex;
            secretariesPage.SelectUsersAtPage(usersOnPage);           
            int actual = secretariesPage.GetShowedUsersAmount();

            if (secretariesPage.GetLastUserIndex(out lastUserIndex))
            {

                if(lastUserIndex >= (int)usersOnPage)
                {
                    expected = (int)usersOnPage;
                }
                else
                {
                    expected = lastUserIndex;
                }

            }
            else
            {
                expected = 0;
                Assert.Fail();
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
            int expected;
            int lastUserIndex;

            if(secretariesPage.GetLastUserIndex(out lastUserIndex))
            {
                expected = lastUserIndex % (int)usersOnPage;
            }
            else
            {
                expected = 0;
                Assert.Fail();
            }

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
            int pagesAmount;
            if (secretariesPage.GetPagesAmount(out pagesAmount) && (pagesAmount>2))
            {
                int expected = (int) usersOnPage;
                int actual = secretariesPage.PrevPage().GetShowedUsersAmount();
                Assert.AreEqual(expected, actual);
            }
            else
            {
                
            }          
        }

    }
}
