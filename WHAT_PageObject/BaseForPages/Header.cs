using OpenQA.Selenium;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public sealed class Header : BasePage
    {
        private readonly By arrowIcon = By.XPath("//body//*[contains(@class,'dropdown-toggler')]");

        private readonly By dropdownMenu = By.XPath("//body//*[contains(@class,'dropdown-list--item')]");

        public Header(IWebDriver driver) : base(driver)
        {
        }

        internal IWebElement FindArrowIcon()
        {
            return driver.FindElement(arrowIcon);
        }

        internal IWebElement FindDropdownItem(string label)
        {
            IList<IWebElement> dropdownItems = driver.FindElements(dropdownMenu);

            foreach (IWebElement dropdownItem in dropdownItems)
            {
                if (dropdownItem.Text.Equals(label))
                {
                    return dropdownItem;
                }
            }

            return null;
        }
    }
}
