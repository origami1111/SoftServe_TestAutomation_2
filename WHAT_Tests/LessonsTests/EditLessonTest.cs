using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
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

        [Test]
        [TestCase("1", "nunit", "2021-06-29T08:00", "×\r\nClose alert\r\nThe lesson has been edit successfully!")]
        public void EditLessonValidTest(string number,string tema,string time,string message)
        {
            string expected = message;

            string actual = lessonsPage
                .ClickEditLesson(number)
                .FillLessonTheme(tema)
                .FillDateTime(time)
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
