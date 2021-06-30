using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace WHAT_PageObject
{
    public class CoursesPage : BasePageWithHeaderSidebar
    {
        private By courseNotFoundLabel = By.XPath("//tbody/h4[contains(.,'Course is not found')]");

        private By addCourseButton = By.XPath("//button/span[contains(.,'Add a course')]");

        private By searchField = By.CssSelector("input[class*='search']");

        private By tableBody = By.CssSelector("tbody");
        
        private By TableCell(int rowNumber, ColumnName columnName) =>
                By.XPath($"//tr[{rowNumber}]/td[{(int)columnName}]");
        
        public CoursesPage(IWebDriver driver) : base(driver)
        {
        }
        
        public enum ColumnName
        {
            Id = 1,
            Title = 2,
            Edit = 3,
        }

        public string ReadCourseName(int rowNumber = 1)
        {
            var cells = driver.FindElements(TableCell(rowNumber, ColumnName.Title));
            
            return cells.Select(cell => cell.Text).FirstOrDefault() ?? string.Empty;
        }

        public bool CourseNotFound()
        {
            return driver.FindElements(courseNotFoundLabel).Count > 0;
        }

        public CourseDetailsPage ClickCourseName(int courseNumber = 1)
        {
            ClickItem(TableCell(courseNumber, ColumnName.Title));

            return new CourseDetailsPage(driver);
        }

        public EditCourseDetailsPage ClickEditIcon(int courseNumber = 1)
        {
            ClickItem(TableCell(courseNumber, ColumnName.Edit));

            return new EditCourseDetailsPage(driver);
        }

        public AddCoursePage ClickAddCourseButton()
        {
            ClickItem(addCourseButton);

            return new AddCoursePage(driver);
        }

        public CoursesPage FillSearchField(string text)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(tableBody));
            FillField(searchField, text);
            
            return this;
        }
    }
}
