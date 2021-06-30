using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace WHAT_PageObject
{
    public class CoursesPage : BasePageWithHeaderSidebar
    {
        #region Locators
        private By pageTitle = By.TagName("h2");

        private By courseNotFoundLabel = By.XPath("//tbody/h4[contains(.,'Course is not found')]");

        private By courseNumberLabel = By.XPath("//span[contains(.,'#')]");

        private By counterOfCoursesLabel = By.CssSelector("span[class='col-2 text-right']");

        private By leftArrow = By.XPath("//button[contains(.,'<')]");

        private By rightArrow = By.XPath("//button[contains(.,'>')]");

        private By addCourseButton = By.XPath("//button/span[contains(.,'Add a course')]");

        private By searchField = By.CssSelector("input[class*='search']");

        private By editIcon = By.CssSelector("use[href='/assets/svg/Edit.svg#Edit']");

        private By table = By.CssSelector("table");

        private By tableHead = By.CssSelector("thead");

        private By tableBody = By.CssSelector("tbody");

        private By rows = By.CssSelector("tr");

        private By idLabel = By.XPath("th/span[@data-sorting-param='id']");

        private By titleLabel = By.XPath("//th/span[@data-sorting-param='name']");

        private By editLabel = By.XPath("//th[contains(.,'Edit')]");
        private By TableCell(int rowNumber, ColumnName columnName) =>
                By.XPath($"//tr[{rowNumber}]/td[{(int)columnName}]");
        #endregion

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

            driver.FindElement(TableCell(courseNumber, ColumnName.Title)).Click();

            return new CourseDetailsPage(driver);
        }

        public EditCourseDetailsPage ClickEditIcon(int courseNumber = 1)
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

        public CoursesPage FillSearchField(string text)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(tableBody));

            IWebElement field = driver.FindElement(searchField);
            field.Click();
            field.Clear();
            field.SendKeys(text);

            return this;
        }
    }
}
