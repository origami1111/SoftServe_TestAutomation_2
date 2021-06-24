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
        public ChangePasswordPage ClickChangePassword()
        {
            driver.FindElement(By.CssSelector(".header__header__dropdown-icon___1CTJ8")).Click();
            driver.FindElement(By.LinkText("Change password")).Click();
            return new ChangePasswordPage(driver);
        }
    }
}
