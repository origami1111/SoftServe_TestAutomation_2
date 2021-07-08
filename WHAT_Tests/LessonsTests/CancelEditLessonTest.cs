using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
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

        [Test]
        [TestCase("1", "nunit", "2021-06-29T08:00")]
        public void CancelEditLessonValidTest(string number, string tema, string time)
        {
            string expected = "http://localhost:8080/lessons";
       
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
