using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class AddCoursePage : BasePageWithHeaderSidebar
    {
        private By courseNameField = By.Id("name");

        private By saveButton = By.CssSelector("button[type='submit']");

        private By cancelButton = By.LinkText("Cancel");

        public AddCoursePage(IWebDriver driver) : base(driver)
        {
        }

        public AddCoursePage FillCourseName(string text)
        {
            FillField(courseNameField, text);
            return this;
        }

        public CoursesPage ClickSaveButton()
        {
            driver.FindElement(saveButton).Click();

            return new CoursesPage(driver);
        }

        public CoursesPage ClickCancelButton()
        {
            driver.FindElement(cancelButton).Click();

            return new CoursesPage(driver);
        }
    }
}
