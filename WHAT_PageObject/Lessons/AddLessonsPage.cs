using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_PageObject
{
    public class AddLessonsPage : BasePageWithHeaderSidebar
    {
        public AddLessonsPage(IWebDriver driver) : base(driver)
        {
        }

        private By lessonsTheme = By.Id("inputLessonTheme");
        private By groupName = By.Id("inputGroupName");
        private By dateTimePicker = By.Id("choose-date/time");
        private By mentor = By.Id("mentorEmail");
        private By cancelButton = By.XPath("//button[contains(.,'Cancel')]");
        private By classRegisterButton = By.CssSelector(".button__default___3hOmG");
        private By saveButton = By.XPath("//button[contains(.,'Save')]");
        public AddLessonsPage FillLessonsTheme(string tema)
        {
            FillField(lessonsTheme, tema);
            return this;
        }
        public AddLessonsPage FillGroupName(string nameOfGroup)
        {
            FillField(groupName, nameOfGroup);
            return this;
        }
        public AddLessonsPage FillDateTime(string dataTime)
        {
            FillField(dateTimePicker,dataTime);
            return this;
        }
        public AddLessonsPage FillMentorEmail(string mentorEmail)
        {
            FillField(mentor, mentorEmail);
            return this;
        }
        public LessonsPage ClickCancelButton()
        {
            ClickItem(cancelButton);
            return new LessonsPage(driver);
        }
        public AddLessonsPage ClickClassRegisterButton()
        {
            ClickItem(classRegisterButton);
            return this;
        }
        public LessonsPage ClickSaveButton()
        {
            ClickItem(saveButton);
            return new LessonsPage(driver);
        }




    }
}
