using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class BasePageWithSidebar : BasePage
    {
        public BasePageWithSidebar(IWebDriver driver) : base(driver)
        {
        }

        private static By sidebarMenuLocator = By.XPath("//span[@class='sidebar__menu-item___1MMsk']");
        
        public bool IsSidebarItemExist(String name)
        {
            return FindSidebarItem(name) != null;
        }
                
        public void ClickSidebarItem(String name)
        {
            IWebElement sidebarItem = FindSidebarItem(name);

            if (sidebarItem != null)
            {
                sidebarItem.Click();
            }
        }

        private IWebElement FindSidebarItem(String name)
        {
            IWebElement element = default;

            IList<IWebElement> sidebarMenu = driver.FindElements(sidebarMenuLocator);

            foreach (IWebElement sidebarItem in sidebarMenu)
            {
                if (sidebarItem.Text.Equals(name))
                {
                    element = sidebarItem;
                }
            }

            return element;
        }
    }
}
