using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;

namespace WHAT_PageFactory
{
    public class LessonsPage : BasePageWithSidebar
    {
        public LessonsPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public CoursesPage ClickCoursesSidebar()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3000);
            ClickSidebarItem("Courses");
            return new CoursesPage(driver);
        }
    }
}
