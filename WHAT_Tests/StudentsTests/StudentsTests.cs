using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;
using System.Threading;
using System;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests
    {
        private IWebDriver driver;

        private StudentsPage studentsPage;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:8080/auth");
            studentsPage = new SignInPage(driver)
                           .SignInAsAdmin("admin.@gmail.com", "admiN_12");
        }


        [SetUp]
        public void ToStudentPage()
        {
            studentsPage.SidebarNavigateTo<StudentsPage>();
        }

        [Test]
        public void FillSearchingField_ValidData([Values("student", "Student")] string forSearch)
        {
            studentsPage.FillSearchingField(forSearch);
        }

        [Test]
        public void RedirectUnassignedUsers()
        {
            studentsPage.ClickAddStudentButton();
            string unassignedUsersURL = "http://localhost:8080/unassigned";
            Assert.AreEqual(unassignedUsersURL, driver.Url);
        }

        [Test]
        public void RedirectStudentsEdit_AnyCard([Values((uint)1)] uint studentNum)
        {
            studentsPage.ClickChoosedStudent(studentNum);
            string studentEditURL = $"http://localhost:8080/students/{studentNum}";
            Assert.AreEqual(studentEditURL, driver.Url);
        }

        //[Test]
        //public void RedirectStudentsEdit_EditIcon([Values((uint)1)] uint studentNum)
        //{
        //    studentsPage.ClickIconEditStudent(studentNum);
        //    string studentEditURL = $"http://localhost:8080/students/edit/{studentNum}";
        //    Assert.AreEqual(studentEditURL, driver.Url);
        //}

        [OneTimeTearDown]
        public void Logout()
        {
            driver.Quit();
        }
    }
}
