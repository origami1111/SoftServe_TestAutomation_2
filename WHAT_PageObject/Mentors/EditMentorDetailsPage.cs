using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class EditMentorDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By mentorDetailsNavLink = By.LinkText("Mentor details");
        private By firstNameField = By.XPath("//input[@id='firstName']");
        private By lastNameField = By.XPath("//input[@id='lastName']");
        private By emailField = By.XPath("//input[@id='email']");
        private By groupsInput = By.XPath("//input[@id='groupsInput']");
        private By addGroupsButton = By.XPath("//button[@id='addGroup']");
        private By coursesInput = By.XPath("//button[@id='coursesInput']");
        private By addCourseButton = By.XPath("//button[@id='addCourse']");
        private By resetButton = By.XPath("//button[@id='resetBtn']");
        private By saveButton = By.XPath("//button[text()='Save']");
        private By disableButton = By.XPath("//button[text()='Disable']");
        #endregion

        public EditMentorDetailsPage(IWebDriver driver) : base(driver)
        {
            
        }

        private void WaitForMentorToLoad()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            IWebElement firstName = wait.Until(e => e.FindElement(firstNameField));
        }

        public MentorDetailsPage ClickMentorDetaisNav()
        {
            ClickItem(mentorDetailsNavLink);
            return new MentorDetailsPage(driver);
        }

        public EditMentorDetailsPage FillFirstNameField(string firstName)
        {
            FillField(firstNameField, firstName);
            return this;
        }

        public EditMentorDetailsPage FillLastNameField(string lastName)
        {
            FillField(lastNameField, lastName);
            return this;
        }

        public EditMentorDetailsPage FillEmailField(string email)
        {
            FillField(emailField, email);
            return this;
        }

        public EditMentorDetailsPage FillGroupsInput(string groupName)
        {
            FillField(groupsInput, groupName);
            return this;
        }

        public EditMentorDetailsPage FillCoursesInput(string courseName)
        {
            FillField(coursesInput, courseName);
            return this;
        }

        public EditMentorDetailsPage ClickAddGroupsButton()
        {
            ClickItem(addGroupsButton);
            return this;
        }

        public EditMentorDetailsPage ClickAddCourseButton()
        {
            ClickItem(addCourseButton);
            return this;
        }

        public EditMentorDetailsPage ClickResetButton()
        {
            ClickItem(resetButton);
            return this;
        }

        public EditMentorDetailsPage ClickSaveButton()
        {
            ClickItem(saveButton);
            return this;
        }

        public EditMentorDetailsPage ClickDisableButton()
        {
            ClickItem(disableButton);
            return this;
        }
    }
}