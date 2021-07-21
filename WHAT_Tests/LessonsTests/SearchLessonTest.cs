using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class SearchLessonTest : TestBase
    {

        private LessonsPage lessonsPage;
        Account account = ReaderFileJson.ReadFileJsonAccounts(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignInPage(driver)
                            .SignInAsMentor(account.Email, account.Password);
        }

        [Test]
        [TestCase("Junit", "1")]
        [TestCase("TestNG", "1")]
        [TestCase("API testing", "1")]
        public void SearchValidLessonTest(string input, string number)
        {
            string expected = input;
            string actual =
                lessonsPage
                .SearchByThemaName(input)
                .GetLessonThemaName(number);

            Assert.AreEqual(expected, actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            lessonsPage.Logout();
        }
    }
}
