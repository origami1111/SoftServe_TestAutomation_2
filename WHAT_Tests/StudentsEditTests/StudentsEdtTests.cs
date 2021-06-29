using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsEdtTests : TestBase
    {

        private StudentsPage studentsPage;
        private StudentsEditPage studentsEditPage;


        [SetUp]
        public void Precondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password);
            studentsPage.SidebarNavigateTo<StudentsPage>();

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }


    }
}
