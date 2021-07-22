using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Tests.LessonsTests;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class EditLessonTest : TestBase
    {
        private LessonsPage lessonsPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignInPage(driver)
                .SignInAsMentor(credentials.Email, credentials.Password);                        
        }

        [TestCaseSource(typeof(TestCasesLessons), nameof(TestCasesLessons.EditLesson))]
        public void EditLessonValidTest(string number, string tema, string time, string expected)
        {
            string actual = lessonsPage
                .ClickEditLesson(number)
                .FillLessonTheme(tema)
                .FillDateTime(time)
                .ClickSaveButton()
                .VerifySuccesMessage();

            StringAssert.Contains(expected, actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            lessonsPage.Logout();
        }
    }
}
