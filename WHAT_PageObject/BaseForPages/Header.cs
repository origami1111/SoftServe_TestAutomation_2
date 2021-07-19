using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public sealed class Header : BasePage
    {
        private readonly By arrowIcon = By.XPath("//body//*[contains(@class,'dropdown-toggler')]/span");

        public Header(IWebDriver driver) : base(driver)
        {
        }

        internal IWebElement FindArrowIcon()
        {
            return driver.FindElement(arrowIcon);
        }

        internal IWebElement FindDropdownItem(string label)
        {
            return driver.FindElement(By.LinkText(label));
        }
    }
}
