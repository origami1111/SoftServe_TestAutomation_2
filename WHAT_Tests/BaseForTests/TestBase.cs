using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace WHAT_Tests
{
    [SetUpFixture]
    public abstract class TestBase 
    {
        protected IWebDriver driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 800);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(ReaderUrlsJSON.ByName("SigninPage"));

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }


    }


}

