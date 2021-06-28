using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;
using System.Threading;
using System;
using WHAT_PageObject.Base;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsEdtTests
    {
        private IWebDriver driver;

        private StudentsEditPage studentsEditPage;
        private StudentsPage studentsPage;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(ReaderUrlsJSON.ByName("SigninPage"));
            studentsPage = new SignIn(driver)
                                .SignInAsAdmin();
            studentsPage.SidebarNavigateTo<StudentsPage>();
            Random randomStudent = new Random();
            studentsEditPage = studentsPage.ClickChoosedStudent((uint)randomStudent.Next(1,11));
        }

        [Test]
        public void Test1()
        {
            //Thread.Sleep(3000);
            Assert.Pass();
        }

        [OneTimeTearDown]
        public void Logout()
        {
            driver.Quit();
        }

    }
}
