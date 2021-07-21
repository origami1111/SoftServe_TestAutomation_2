using NUnit.Framework;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class LessonsPaginationTest : TestBase
    {

        private LessonsPage lessonsPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignInPage(driver)
                            .SignInAsMentor(credentials.Email, credentials.Password);
        }

        [Test]
        [TestCase("1")]
        public void LessonsPaginationNextTest(string number)
        {

            int before = Convert.ToInt32(lessonsPage.GetLessonById(number));

            int after = Convert.ToInt32(lessonsPage.ClickNextPageOnPagination().GetLessonById(number));

            Assert.AreNotEqual(before, after);
        }

        [Test]
        [TestCase("1")]
        public void LessonsPaginationPrevTest(string number)
        {
            lessonsPage.ClickNextPageOnPagination();

            int before = Convert.ToInt32(lessonsPage.GetLessonById(number));

            int after = Convert.ToInt32(lessonsPage.ClickNextPageOnPagination().GetLessonById(number));

            Assert.AreNotEqual(before, after);
        }

        [TearDown]
        public void SetPostConditions()
        {
            lessonsPage.Logout();
        }
    }
}
