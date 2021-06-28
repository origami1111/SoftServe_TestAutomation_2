﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public abstract class BasePageWithHeaderSidebar : BasePage
    {
        private readonly Sidebar sidebar;

        private readonly Header header;

        private readonly Dictionary<Type, string> sidebarLabels = new Dictionary<Type, string>()
        {
            [typeof(StudentsPage)] = "Students",
            [typeof(MentorsPage)] = "Mentors",
            [typeof(SecretariesPage)] = "Secretaries",
            [typeof(LessonsPage)] = "Lessons",
            //[typeof(GroupsPage)] = "Groups",
            [typeof(CoursesPage)] = "Courses",
            //[typeof(SchedulePage)] = "Schedule",
            [typeof(UnassignedUsers)] = "Assigment",
            [typeof(SupportPage)] = "Support"
        };

        protected BasePageWithHeaderSidebar(IWebDriver driver) : base(driver)
        {
            sidebar = new Sidebar(driver);
            header = new Header(driver);
        }

        public T SidebarNavigateTo<T>() where T : BasePageWithHeaderSidebar
        {
            ClickSidebarItem(sidebarLabels[typeof(T)]);

            var nextPage = (T)Activator.CreateInstance(typeof(T), driver);
            return nextPage;
        }

        public void ClickSidebarItem(string label)
        {
            IWebElement sidebarItem = sidebar.FindSidebarItem(label);
            sidebarItem?.Click();
        }

        public void ClickArrowIcon()
        {
            header.FindArrowIcon().Click();
        }

        public void ClickDropdownItem(string label)
        {
            IWebElement dropdownItem = header.FindDropdownItem(label);
            dropdownItem?.Click();
        }

        public ChangePasswordPage ClickChangePassword()
        {
            driver.FindElement(By.XPath("//span[contains(.,\'▼\')]")).Click();
            driver.FindElement(By.LinkText("Change password")).Click();
            return new ChangePasswordPage(driver);
        }
        
        public SignInPage Logout()
        {
            ClickArrowIcon();
            ClickDropdownItem("Log Out");
            return new SignInPage(driver);
        }
        public MyProfilePage ViewMyProfile()
        {
            driver.FindElement(By.XPath("//span[contains(.,\'▼\')]")).Click();
            driver.FindElement(By.LinkText("My profile")).Click();
            return new MyProfilePage(driver);
        }
    }
}
