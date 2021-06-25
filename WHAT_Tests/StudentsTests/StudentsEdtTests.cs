using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;
using System.Threading;
using System;

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
            driver.Navigate().GoToUrl("http://localhost:8080/auth");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(40);
            studentsPage = new SignInPage(driver)
                           .SignInAsAdmin("admin.@gmail.com", "admiN_12");
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
