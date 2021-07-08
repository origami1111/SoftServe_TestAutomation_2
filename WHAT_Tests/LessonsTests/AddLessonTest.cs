using NUnit.Framework;
using System;
using System.Threading;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
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

        [Test]
        public void AddLessonWithValidDataTest()
        {
            Lesson lesson = new Lesson("Test" + DateTime.Now.ToString(), "Advanced", "2021-06-28T09:00", "MentoR01@gmail.com");

            string expected = "×\r\nClose alert\r\nThe lesson has been added successfully!";

            string actual = lessonsPage
              .ClickAddLessonButton()
              .FillLessonsTheme(lesson.GetLessonThema())
              .FillGroupName(lesson.GetGroupName())
              .FillDateTime(lesson.GetDateTime())
              .FillMentorEmail(lesson.GetMentorEmail())
              .ClickClassRegisterButton()
              .ClickSaveButton()
              .VerifySuccesMessage();
              
            Assert.AreEqual(expected,actual);
        }

        [TearDown]
        public void SetPostConditions()
        {
            lessonsPage.Logout();
        }

    }
}
