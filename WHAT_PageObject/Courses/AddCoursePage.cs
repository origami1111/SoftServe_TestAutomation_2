using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class AddCoursePage : BasePageWithHeaderSidebar
    {
        private By courseNameField = By.TagName("input");

        private By saveButton = By.CssSelector("button[type='submit']");

        private By cancelButton = By.LinkText("Cancel");

        private By addCourseForm = By.XPath("//body//form[h3/text()='Add a course']");

        private By errorMessage = By.XPath("//div/p[contains(@class,'add-course__error')]");

        private IWebElement addCourseFormElement;

        public AddCoursePage(IWebDriver driver) : base(driver)
        {
            addCourseFormElement = driver.FindElement(addCourseForm);
        }

        public AddCoursePage FillCourseNameField(string text)
        {
            IWebElement field = addCourseFormElement.FindElement(courseNameField);

            field.Click();
            field.Clear();
            field.SendKeys(text);

            return this;
        }

        public AddCoursePage DeleteTextWithBackspaces(int backspacesNumber)
        {
            var field = addCourseFormElement.FindElement(courseNameField);

            for (int i = 0; i < backspacesNumber; i++)
            {
                field.SendKeys(Keys.Backspace);
            }

            return this;
        }

        public CoursesPage ClickSaveButton()
        {
            addCourseFormElement.FindElement(saveButton).Click();

            return new CoursesPage(driver);
        }

        public CoursesPage ClickCancelButton()
        {
            addCourseFormElement.FindElement(cancelButton).Click();

            return new CoursesPage(driver);
        }

        public string GetErrorMessage()
        {
            return addCourseFormElement.FindElement(errorMessage).Text;
        }

        public bool IsSaveButtonDisabled()
        {
            return !addCourseFormElement.FindElement(saveButton).Enabled;
        }
    }
}
