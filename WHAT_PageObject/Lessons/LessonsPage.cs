using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class LessonsPage : BasePageWithHeaderSidebar
    {
        private By header = By.XPath("//span[contains(.,'▼')]");
        private By changePassword = By.LinkText("Change password");
        public LessonsPage(IWebDriver driver) : base(driver)
        {
        }
        public ChangePasswordPage ClickChangePassword()
        {
            ClickItem(header);
            ClickItem(changePassword);
            return new ChangePasswordPage(driver);
        }
        public CoursesPage ClickCoursesSidebar()
        {
            
            ClickSidebarItem("Courses");
            return new CoursesPage(driver);
        }
    }
}
