using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class CoursesPage : BasePageWithHeaderSidebar
    {
        private readonly Table table;

        #region Locators
        private By pageTitle = By.TagName("h2");
        
        private By courseNumberLabel = By.XPath("//span[contains(.,'#')]");

        private By counterOfCoursesLabel = By.CssSelector("span[class='col-2 text-right']");

        private By leftArrow = By.XPath("//button[contains(.,'<')]");

        private By rightArrow = By.XPath("//button[contains(.,'>')]");

        private By addCourseButton = By.XPath("//button/span[contains(.,'Add a course')]");

        private By pencilLink = By.CssSelector("use[href='/assets/svg/Edit.svg#Edit']");
        #endregion


        private By CourseName(string courseNumber) =>
            By.XPath($"//tr[@data-student-id={courseNumber}]/td[2]");

        private By pensilLink(string courseNumber) =>
            By.XPath($"//tr[@data-student-id={courseNumber}]/td[2]");

        public CoursesPage(IWebDriver driver) : base(driver)
        {
        }

        public string ReadCourseName(string courseNumber)
        {
            var course = driver.FindElement(CourseName(courseNumber));

            return course.Text;
        }

        public CourseDetailsPage ClickCourseName(string courseNumber)
        {
            var course = driver.FindElement(CourseName(courseNumber));
            course.Click();

            return new CourseDetailsPage(driver);
        }

        public CoursesPage CountTotalNumber(out int totalNumber)
        {
            totalNumber = driver.FindElements(By.XPath("//table[@id='Edit']//tr")).Count;

            return this;
        }

        
    }
}
