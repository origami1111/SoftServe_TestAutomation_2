using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class Table : BasePage
    {
        #region Locators
        private By table = By.Name("table");

        private By tableHead = By.Name("thead");

        private By tableBody = By.Name("tbody");

        private By rows = By.Name("tr");

        private By idLabel = By.XPath("th/span[@data-sorting-param='id']");

        private By titleLabel = By.XPath("//th/span[@data-sorting-param='name']");

        private By editLabel = By.XPath("//th[contains(.,'Edit')]");
        #endregion

        public Table(IWebDriver driver) : base(driver)
        {
        }

        private By CourseName(string rowNumber) =>
            By.XPath($"//tr[{rowNumber}]/td[2]");

        private By pencilLink(string rowNumber) =>
            By.XPath($"//tr[{rowNumber}]/td[3]");

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

        public CourseDetailsPage ClickPencilLink(int rowNumber)
        {
            IWebElement table = driver.FindElement(this.table);
            var rows = table.FindElements(this.rows);
            var rowTds = rows[rowNumber - 1].FindElements(By.TagName("td"));

            foreach (var td in rowTds)
            {
                var pencil = td.FindElement(By.Id("Edit"));
                pencil.Click();
                break;
            }

            return new CourseDetailsPage(driver);
        }
    }
}
