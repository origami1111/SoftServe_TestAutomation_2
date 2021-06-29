using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class StudentsTests_VerifyRedirection
    {

        private StudentsPage studentsPage;
        private IWebDriver driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 800);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(ReaderUrlsJSON.ByName("SigninPage"));
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            studentsPage = new SignInPage(driver)
                                .SignInAsAdmin(credentials.Email, credentials.Password);
        }

        [SetUp]
        public void ToStudentPage()
        {
            studentsPage.SidebarNavigateTo<StudentsPage>();
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void RedirectUnassignedUsers()
        {
            studentsPage.ClickAddStudentButton();
            string unassignedUsersURL = ReaderUrlsJSON.ByName("UnassignedUsersPage").ToString();
            Assert.AreEqual(unassignedUsersURL, driver.Url);
        }
        //uint[] arr = Enumerable.Range(1, 10).Cast<uint>().ToArray();

        [Test]
        public void RedirectStudentsEdit_AnyCard([Values((uint)1)] uint studentNum)
        {
            studentsPage.ClickChoosedStudent(studentNum);
            string studentEditURL = ReaderUrlsJSON.ByNameAndNumber("StudentsPage", studentNum).ToString();
            Assert.AreEqual(studentEditURL, driver.Url);
        }

        //[Test]
        //public void RedirectStudentsEdit_EditIcon([Values((uint)1)] uint studentNum)
        //{
        //    studentsPage.ClickIconEditStudent(studentNum);
        //    string studentEditURL = ReaderUrlsJSON.ByNameAndNumber("StudentsEditPage", studentNum).ToString();
        //    Assert.AreEqual(studentEditURL, driver.Url);
        //}

    }
}
