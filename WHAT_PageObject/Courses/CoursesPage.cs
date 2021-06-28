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

        public enum ColumnName
        {
            Id = 1,
            Title = 2,
            Edit = 3,
        }

        private By TableCell(int rowNumber, ColumnName columnName) =>
            By.XPath($"//tr[{rowNumber}]/td[{(int)columnName}]");

        public CoursesPage(IWebDriver driver) : base(driver)
        {
        }

        public string ReadCourseName(int rowNumber)
        {
            var course = driver.FindElement(TableCell(rowNumber, ColumnName.Title));

            return course.Text;
        }

        public CourseDetailsPage ClickCourseName(int courseNumber)
        {

            driver.FindElement(TableCell(courseNumber, ColumnName.Title)).Click();

            return new CourseDetailsPage(driver);
        }

        public EditCourseDetailsPage ClickPencilLink(int courseNumber)
        {

            driver.FindElement(TableCell(courseNumber, ColumnName.Edit)).Click();

            return new EditCourseDetailsPage(driver);
        }

        public CoursesPage CountTotalNumber(out int totalNumber)
        {
            totalNumber = driver.FindElements(By.XPath("//table[@id='Edit']//tr")).Count;

            return this;
        }

        public AddCoursePage ClickAddCourseButton()
        {
            driver.FindElement(addCourseButton).Click();

            return new AddCoursePage(driver);
        }
    }
}
