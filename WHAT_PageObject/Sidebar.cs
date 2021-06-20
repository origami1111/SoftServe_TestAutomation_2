using OpenQA.Selenium;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class Sidebar : BasePage
    {
        private By sidebarMenuLocator = By.XPath("//span[@class='sidebar__menu-item___1MMsk']");

        public Sidebar(IWebDriver driver) : base(driver)
        {
        }

        public void ClickSidebarItem(string sidebarLabel)
        {
            IWebElement sidebarItem = FindSidebarItem(sidebarLabel);

            sidebarItem?.Click();
        }

        private IWebElement FindSidebarItem(string sidebarLabel)
        {
            IWebElement element = default;

            IList<IWebElement> sidebarItems = driver.FindElements(sidebarMenuLocator);

            foreach (IWebElement sidebarItem in sidebarItems)
            {
                if (sidebarItem.Text.Equals(sidebarLabel))
                {
                    element = sidebarItem;
                }
            }

            return element;
        }
    }
}
