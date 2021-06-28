using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class AddLessonTest : TestBase
    {
        private LessonsPage lessonsPage;

        [SetUp]
        public void SetupPage()
        {
            lessonsPage = new SignIn(driver).SignInAsMentor();
        }

        [Test]
        public void AddLessonWithValidDataTest()
        {
            
            Lesson lesson = new Lesson("Test"+DateTime.Now.ToString(), "Advanced", "2021-06-28T09:00", "MentoR01@gmail.com");
            int before = lessonsPage.GetCountLessons();

            lessonsPage.ClickAddLessonButton()
              .FillLessonsTheme(lesson.GetLessonThema())
              .FillGroupName(lesson.GetGroupName())
              .FillDateTime(lesson.GetDateTime())
              .FillMentorEmail(lesson.GetMentorEmail())
              .ClickClassRegisterButton()
              .ClickSaveButton()
              .RefreshPage();


            int after = lessonsPage.GetCountLessons();
            Assert.AreEqual(before + 1 , after);
        }

        [TearDown]
        public void SetPostConditions()
        {
            lessonsPage.Logout();
        }

    }
}
