using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using WHAT_Utilities;

namespace WHAT_PageObject
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        List<BasePage> pages = new List<BasePage>();

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
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
        public T SoftAssertAdd<T>(SoftAssert sAssert, Object expected, Object actual, string errorMessage = "") where T : BasePage
        {
            sAssert.Add(expected, actual, errorMessage);
            return GetPageInstance<T>(driver);
        }

        public T SoftAssertAll<T>(SoftAssert sAssert) where T : BasePage
        {
            sAssert.AssertAll();
            return GetPageInstance<T>(driver);
        }

        public T ForEachEntry<T,A>(IEnumerable<A> collection, T page, Action<A> action) where T : BasePage
        {
            foreach (A item in collection)
            {
                action(item);
            }            
            return GetPageInstance<T>(driver);
        }

        public T WaitUntilElementLoads<T>(By locator) where T : BasePage
        {
            wait.Until(e => e.FindElement(locator));
            return GetPageInstance<T>(driver);
        }

        public T ChangePageInstance<T>() where T : BasePage
        {
            return GetPageInstance<T>(driver);
        }

        protected T GetPageInstance<T>(params object[] args) where T : BasePage
        {
            T foundPage = null;
            foreach (BasePage page in pages)
            {
                if (page is T)
                {
                    foundPage = (T)page;
                    break;
                }
            }
            if (foundPage == null)
            {
                foundPage = (T)Activator.CreateInstance(typeof(T), args);
                pages.Add(foundPage);
            }
            return foundPage;
        }
    }
}
