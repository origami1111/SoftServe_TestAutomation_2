using OpenQA.Selenium;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public sealed class Sidebar : BasePage
    {
        private readonly By sidebarMenu = By.XPath("//span[@class='sidebar__menu-item___1MMsk']");

        public Sidebar(IWebDriver driver) : base(driver)
        {
        }

        internal IWebElement FindSidebarItem(string label)
        {
            IList<IWebElement> sidebarItems = driver.FindElements(sidebarMenu);

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
