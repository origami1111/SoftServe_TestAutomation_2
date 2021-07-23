using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Tests.LessonsTests;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class CancelEditLessonTest : TestBase
    {
        private LessonsPage lessonsPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignInPage(driver)
                .SignInAsMentor(credentials.Email, credentials.Password);                 
        }

        [TestCaseSource(typeof(TestCasesLessons), nameof(TestCasesLessons.CancelEditLesson))]
        public void CancelEditLessonValidTest(string number, string tema, string time, string expected)
        {
            lessonsPage
              .ClickEditLesson(number)
              .FillLessonTheme(tema)
              .FillDateTime(time)
              .ClickCancelButton();

            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            lessonsPage.Logout();
        }
    }
}
