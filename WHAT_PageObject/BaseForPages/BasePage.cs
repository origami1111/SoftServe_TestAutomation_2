using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public abstract class BasePage
    {
        protected IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected void FillField(By locator, string text)
        {
            var field = driver.FindElement(locator);
            field.Click();
            field.Clear();
            field.SendKeys(Keys.Control + "a");
            field.SendKeys(Keys.Delete);
            field.SendKeys(text);

        }

        protected void ClickItem(By locator)
        {
            driver.FindElement(locator).Click();
        }

        protected void ActionClickItem(By locator)
        {
            IWebElement element = driver.FindElement(locator);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Click().Perform();
        }

        public T WaitUntilElementLoads<T>(By locator) where T : BasePage
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            wait.Until(e => e.FindElement(locator));
            return GetPageInstance<T>(driver);
        }

        public T ChangePageInstance<T>() where T : BasePage
        {
            return GetPageInstance<T>(driver);
        }

        public T Log<T>(string text) where T : BasePage
        {

            return GetPageInstance<T>(driver);
        }

        protected T GetPageInstance<T>(params object[] args) where T : BasePage
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}
