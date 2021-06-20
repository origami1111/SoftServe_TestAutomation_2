using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class LessonsPage : Sidebar
    {
        public LessonsPage(IWebDriver driver) : base(driver)
        {
        }

        public CoursesPage ClickCoursesSidebar()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3000);
            ClickSidebarItem("Courses");
            return new CoursesPage(driver);
        }
    }
}
