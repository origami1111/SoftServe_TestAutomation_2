using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;

namespace WHAT_PageFactory
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        
        protected WebDriverWait wait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            PageFactory.InitElements(driver, this);
        }
    }
}
