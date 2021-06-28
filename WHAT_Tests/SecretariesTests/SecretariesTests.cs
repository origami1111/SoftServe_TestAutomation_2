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
            secretariesPage = new SignIn(driver)
                            .SignInAsAdmin()
                            .SidebarNavigateTo<SecretariesPage>();
        }

        [TearDown]
        public void TearDown()
        {
            secretariesPage.Logout();
        }

        //wwwwwwwwwww
    }
}
