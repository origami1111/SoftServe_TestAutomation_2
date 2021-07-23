using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Tests.LessonsTests;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    public class AddLessonTest : TestBase
    {
        private LessonsPage lessonsPage;
        Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Mentor);

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignInPage(driver)
                .SignInAsMentor(credentials.Email, credentials.Password);             
        }

        [TestCaseSource(typeof(TestCasesLessons), nameof(TestCasesLessons.AddLesson))]
        public void AddLessonWithValidDataTest(string thema,string groupName,string date,string mentorEmail, string expected)
        {
            string actual = lessonsPage
              .ClickAddLessonButton()
              .FillLessonsTheme(thema)
              .FillGroupName(groupName)
              .FillDateTime(date)
              .FillMentorEmail(mentorEmail)
              .ClickClassRegisterButton()
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
