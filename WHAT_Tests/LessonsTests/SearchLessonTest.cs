using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
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

        [Test]
        [TestCase("Junit","1")]
        [TestCase("TestNG","1")]
        [TestCase("API testing","1")]
        public void SearchValidLessonTest(string input,string number)
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
