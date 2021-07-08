using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [SetUpFixture]
    public abstract class TestBase
    {
        protected IWebDriver driver;
        public readonly string LinksPath = @"DataFiles/Links.json";
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(ReaderUrlsJSON.ByName("SigninPage", LinksPath));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}

