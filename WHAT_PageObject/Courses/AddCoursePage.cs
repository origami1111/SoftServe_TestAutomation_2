using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_PageObject
{
    public class AddCoursePage : BasePageWithHeaderSidebar
    {
        private By courseName = By.Id("name");

        private By saveButton = By.CssSelector("button[type='submit']");
        
        public AddCoursePage(IWebDriver driver) : base(driver)
        {
        }

        public AddCoursePage FillCourseNameField(string text)
        {
            FillField(courseName, text);
            return this;
        }

        public CoursesPage ClickSaveButton()
        {
            driver.FindElement(saveButton).Click();

            return new CoursesPage(driver);
        }
    }
}
