using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Tests.LessonsTests;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class SearchLessonTest : TestBase
    {
        private LessonsPage lessonsPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignInPage(driver)
                .SignInAsMentor(credentials.Email, credentials.Password);
        }

        [TestCaseSource(typeof(TestCasesLessons), nameof(TestCasesLessons.SearchLesson))]
        public void SearchValidLessonTest(string input, string number)
        {
            string expected = input;
            string actual = lessonsPage
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
