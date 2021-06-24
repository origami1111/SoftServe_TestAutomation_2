using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public abstract class BasePageWithHeaderSidebar : BasePage
    {
        private readonly Sidebar sidebar;

        private readonly Dictionary<Type, string> sidebarLabels = new Dictionary<Type, string>()
        {
            [typeof(CoursesPage)] = "Students",
            // [typeof(MentorsPage)] = "Mentors",
            // [typeof(SecretariesPage)] = "Secretaries",
            [typeof(LessonsPage)] = "Lessons",
            // [typeof(GroupsPage)] = "Groups",
            [typeof(CoursesPage)] = "Courses",
            // [typeof(SchedulePage)] = "Schedule",
            // [typeof(AssigmentPage)] = "Assigment",
            // [typeof(SupportPage)] = "Support"
        };

        protected BasePageWithHeaderSidebar(IWebDriver driver) : base(driver)
        {
            sidebar = new Sidebar(driver);
        }

        public T SidebarNavigateTo<T>() where T : BasePage
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3000);

            ClickSidebarItem(sidebarLabels[typeof(T)]);

            var nextPage = (T)Activator.CreateInstance(typeof(T), driver); ;
            return nextPage;
        }

        public void ClickSidebarItem(string label)
        {
            IWebElement sidebarItem = sidebar.FindSidebarItem(label);
            sidebarItem?.Click();
        }
    }
}
