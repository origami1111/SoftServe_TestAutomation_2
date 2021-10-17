using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using WHAT_Utilities;

namespace WHAT_PageObject
{
    public class EditMentorDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By form = By.XPath(Locators.EditMentorDetailsPage.FORM);
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
        private By firstNameError = By.XPath(Locators.EditMentorDetailsPage.FIRST_NAME_ERROR);
        private By lastNameError = By.XPath(Locators.EditMentorDetailsPage.LAST_NAME_ERROR);
        private By emailError = By.XPath(Locators.EditMentorDetailsPage.EMAIL_ERROR);
        #endregion

        #region CONSTRUCTOR

        public EditMentorDetailsPage(IWebDriver driver) : base(driver)
        {
            
        }

        #endregion

        #region ACTIONS

        public MentorDetailsPage ClickMentorDetailsNav()
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

        #endregion

        #region Waits

        public EditMentorDetailsPage WaitUntilFormLoads()
        {
            WaitUntilElementLoads<EditMentorDetailsPage>(form);
            return this;
        }

        #endregion

        #region VERIFICATION

        public EditMentorDetailsPage VerifyFields(List<TestData> firstNames, List<TestData> lastNames, List<TestData> emails)
        {
            Assert.Multiple(() =>
            {
                foreach (TestData data in firstNames)
                {
                    FillFirstNameField(data.Value);
                    VerifyFirstNameError(data.Result);
                    VerifySaveButtonEnabled(false);
                }
                driver.FindElement(resetButton).Click();
                foreach (TestData data in lastNames)
                {
                    FillLastNameField(data.Value);
                    VerifyLastNameError(data.Result);
                    VerifySaveButtonEnabled(false);
                }
                driver.FindElement(resetButton).Click();
                foreach (TestData data in emails)
                {
                    FillEmailField(data.Value);
                    VerifyEmailError(data.Result);
                    VerifySaveButtonEnabled(false);
                }
            });
            return this;
        }

        public EditMentorDetailsPage VerifyFirstNameError(string expected)
        {
            string assertMessage = AssertionMessages.EditMentorsDetailsPage.FIRST_NAME_ERROR_MESSAGE;
            string actual = driver.FindElement(firstNameError).Text.ToString();
            Assert.AreEqual(expected, actual, assertMessage);
            return this;
        }

        public EditMentorDetailsPage VerifyLastNameError(string expected)
        {
            string assertMessage = AssertionMessages.EditMentorsDetailsPage.LAST_NAME_ERROR_MESSAGE;
            string actual = driver.FindElement(lastNameError).Text.ToString();
            Assert.AreEqual(expected, actual, assertMessage);
            return this;
        }
        public EditMentorDetailsPage VerifyEmailError(string expected)
        {
            string assertMessage = AssertionMessages.EditMentorsDetailsPage.EMAIL_ERROR_MESSAGE;
            string actual = driver.FindElement(emailError).Text.ToString();
            Assert.AreEqual(expected, actual, assertMessage);
            return this;
        }

        public EditMentorDetailsPage VerifySaveButtonEnabled(bool expected = true)
        {
            string assertMessage = "Save button verification";
            bool actual = driver.FindElement(saveButton).Enabled;
            Assert.AreEqual(expected, actual, assertMessage);
            return this;
        }

        public EditMentorDetailsPage VerifyFirstNameFilled(string expected)
        {
            string actual = driver.FindElement(firstNameField).GetAttribute("Value");
            Assert.AreEqual(expected, actual);
            return this;
        }

        public EditMentorDetailsPage VerifyLastNameFilled(string expected)
        {
            string actual = driver.FindElement(lastNameField).GetAttribute("Value");
            Assert.AreEqual(expected, actual);
            return this;
        }

        public EditMentorDetailsPage VerifyEmailFilled(string expected)
        {
            string actual = driver.FindElement(emailField).GetAttribute("Value");
            Assert.AreEqual(expected, actual);
            return this;
        }

        #endregion

        public string GetFirstNameError()
        {
            return driver.FindElement(firstNameError).Text.ToString();
        }
        public string GetLastNameError()
        {
            return driver.FindElement(lastNameError).Text.ToString();
        }
        public string GetEmailError()
        {
            return driver.FindElement(emailError).Text.ToString();
        }
        public bool IsSaveButtonEnabled()
        {
            return driver.FindElement(saveButton).Enabled;
        }
    }
}