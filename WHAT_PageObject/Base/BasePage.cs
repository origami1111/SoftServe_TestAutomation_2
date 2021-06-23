using OpenQA.Selenium;

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
            driver.FindElement(locator).Click();
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(text);
        }

        protected void ClickItem(By locator)
        {
            driver.FindElement(locator).Click();
        }

        private By headerBarLocator = By.CssSelector(".header__header__account-user--fullname___2kSON");
        private By logOutLocator = By.LinkText("Log Out");

        public void Logout()
        {
            ClickItem(headerBarLocator);
            ClickItem(logOutLocator);
        }
    }
}
