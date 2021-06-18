using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHAT_PageObject
{
    public class LessonsPage : BasePageWithSidebar
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
