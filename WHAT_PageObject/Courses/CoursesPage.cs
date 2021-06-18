using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHAT_PageObject
{
    public class CoursesPage : BasePageWithSidebar
    {
        public CoursesPage(IWebDriver driver) : base(driver)
        {
        }

        public string ReadCourseName(string courseNumber)
        {
            var course = driver.FindElement(By.XPath($"//tr[@data-student-id={courseNumber}]/td[2]"));

            return course.Text;
        }
        public CourseDetailsPage ClickCourseName(string courseNumber)
        {
            var course = driver.FindElement(By.XPath($"//tr[@data-student-id={courseNumber}]/td[2]"));
            course.Click();

            return new CourseDetailsPage(driver);
        }
    }
}
