using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class Sidebar : BasePage
    {
        

        private By sidebarMenuLocator = By.XPath("//span[@class='sidebar__menu-item___1MMsk']");

        public Sidebar(IWebDriver driver) : base(driver)
        {
        }

        internal IWebElement FindSidebarItem(string label)
        {
            IList<IWebElement> sidebarItems = driver.FindElements(sidebarMenuLocator);

            foreach (IWebElement sidebarItem in sidebarItems)
            {
                if (sidebarItem.Text.Equals(label))
                {
                    return sidebarItem;
                }
            }

            return null;
        }
    }
}
