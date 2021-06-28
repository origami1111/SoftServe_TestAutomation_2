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

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl("http://localhost:8080/");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }


    }


}

