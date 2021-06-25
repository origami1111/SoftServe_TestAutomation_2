using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;

namespace WHAT_PageFactory
{
    public class Sidebar : BasePage
    {
        private readonly Dictionary<Type, string> sidebarLabels = new Dictionary<Type, string>()
        {
            [typeof(CoursesPage)] = "Students",
            //[typeof(MentorsPage)] = "Mentors",
        };

        [FindsBy(How = How.XPath, Using = "//span[@class='sidebar__menu-item___1MMsk']")]
        [CacheLookup]
        private IList<IWebElement> sidebarItems;

        public Sidebar(IWebDriver driver) : base(driver)
        {
        }

        public T NavigateTo<T>() where T : BasePage
        {
            return New<T>();
        }

        public T SidebarNavigateTo<T>() where T : BasePage
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3000);

            var nextPage = New<T>();
            return nextPage;
        }

        public void ClickSidebarItem(string sidebarLabel)
        {
            IWebElement sidebarItem = FindSidebarItem(sidebarLabel);

            sidebarItem?.Click();
        }

        private T New<T>() where T : BasePage
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3000);
            var newPage = (T)Activator.CreateInstance(typeof(T), driver);
            return newPage;
        }

        private IWebElement FindSidebarItem(string sidebarLabel)
        {
            foreach (IWebElement sidebarItem in sidebarItems)
            {
                if (sidebarItem.Text.Equals(sidebarLabel))
                {
                    return sidebarItem;
                }
            }

            return null;
        }
    }
}
