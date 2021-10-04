using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class EditMentorDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By mentorDetailsNavLink = By.LinkText(Locators.EditMentorDetailsPage.MENTOR_DETAILS_LINK);
        private By firstNameField = By.XPath(Locators.EditMentorDetailsPage.FIRST_NAME);
        private By lastNameField = By.XPath(Locators.EditMentorDetailsPage.LAST_NAME);
        private By emailField = By.XPath(Locators.EditMentorDetailsPage.EMAIL);
        private By groupsInput = By.XPath(Locators.EditMentorDetailsPage.GROUPS);
        private By addGroupsButton = By.XPath(Locators.EditMentorDetailsPage.ADD_GROUP_BUTTON);
        private By coursesInput = By.XPath(Locators.EditMentorDetailsPage.COURSES);
        private By addCourseButton = By.XPath(Locators.EditMentorDetailsPage.ADD_COURSE_BUTTON);
        private By resetButton = By.XPath(Locators.EditMentorDetailsPage.RESET_BUTTON);
        private By saveButton = By.XPath(Locators.EditMentorDetailsPage.SAVE_BUTTON);
        private By disableButton = By.XPath(Locators.EditMentorDetailsPage.DISABLE_BUTTON);
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