using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_PageObject
{
    public class EditCourseDetailsPage : BasePageWithHeaderSidebar
    {
        private By editForm = By.XPath("//body//form[@action]");

        private By courseNameField = By.CssSelector("input[name='name']");

        private By errorMessage = By.XPath("//div/p[contains(@class,'add-course__error')]");

        private By deleteButton = By.CssSelector("button[type='button']");

        private By clearButton = By.CssSelector("button[type='reset']");

        private By saveButton = By.CssSelector("button[type='submit']");

        private IWebElement EditFormElement;

        public EditCourseDetailsPage(IWebDriver driver) : base(driver)
        {
            EditFormElement = driver.FindElement(editForm);
        }

        public EditCourseDetailsPage FillCourseNameField(string text)
        {
            IWebElement field = EditFormElement.FindElement(courseNameField);

            field.Click();
            field.Clear();
            field.SendKeys(text);

            return this;
        }

        public EditCourseDetailsPage DeleteTextWithBackspaces(int backspacesNumber = 1)
        {
            var field = EditFormElement.FindElement(courseNameField);

            for (int i = 0; i < backspacesNumber; i++)
            {
                field.SendKeys(Keys.Backspace);
            }

            return this;
        }

        public CoursesPage ClickSaveButton()
        {
            EditFormElement.FindElement(saveButton).Click();

            return new CoursesPage(driver);
        }

        public EditCourseDetailsPage ClickClearButton()
        {
            EditFormElement.FindElement(clearButton).Click();

            return new EditCourseDetailsPage(driver);
        }

        public string GetErrorMessage()
        {
            return EditFormElement.FindElement(errorMessage).Text;
        }
        public string GetCourseName()
        {
            return EditFormElement
                        .FindElement(courseNameField)
                        .GetAttribute("value");
        }

        public bool IsSaveButtonDisabled()
        {
            return !EditFormElement.FindElement(saveButton).Enabled;
        }
    }
}
