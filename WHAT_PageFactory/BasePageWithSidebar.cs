using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;

namespace WHAT_PageFactory
{
    public class BasePageWithSidebar : BasePage
    {
        [FindsBy(How = How.XPath, Using = "//span[@class='sidebar__menu-item___1MMsk']")]
        [CacheLookup]
        private IList<IWebElement> sidebarMenu;

        public BasePageWithSidebar(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
        
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
