using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;


namespace WHAT_Tests
{
    [TestFixture]
    public class SecretariesTests : TestBase
    {
        private SecretariesPage secretariesPage;

        [SetUp]
        public void Setup()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            
            secretariesPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>();
        }

        [TearDown]
        public void Down()
        {
            secretariesPage.Logout();
        }

        [Test]        
        public void VerifyUsersOnFirstPage()
        {
            int expected = secretariesPage.GetUsersOnPage();
            int actual = secretariesPage.GetShowedUsersAmount();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyUsersOnLastPage()
        {
            int expected = secretariesPage.GetUsersOnPage();
            int actual = secretariesPage.GetShowedUsersAmount();
            // ReaderFileCSV.ReadFileListCredentials("secretary_active.csv").Count; // Читать из файла
            Assert.AreEqual(expected, actual);
        }
    }
}
