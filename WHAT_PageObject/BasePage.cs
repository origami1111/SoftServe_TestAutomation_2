using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace WHAT_PageObject
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected IJavaScriptExecutor javaScript;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

    }
}
