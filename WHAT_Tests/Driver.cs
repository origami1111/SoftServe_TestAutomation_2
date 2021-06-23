using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace WHAT_Tests
{
    class Driver
    {

        public IWebDriver SetDriver(IWebDriver driver)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.FullScreen();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("http://localhost:8080/");
            return driver;
        }

        public IWebDriver StopDriver(IWebDriver driver)
        {
            driver.Quit();
            return driver;
        }
    }
}
