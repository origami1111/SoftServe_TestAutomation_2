﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class MentorDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By editDetailsNavLink = By.LinkText(Locators.MentorDetailsPage.EDIT_MENTOR_LINK);
        private By mentorFirstName = By.XPath(Locators.MentorDetailsPage.FIRST_NAME);
        private By mentorLastName = By.XPath(Locators.MentorDetailsPage.LAST_NAME);
        private By mentorEmail = By.XPath(Locators.MentorDetailsPage.EMAIL);
        #endregion

        public MentorDetailsPage(IWebDriver driver) : base(driver)
        {

        }
        private void WaitForMentorToLoad()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            IWebElement firstName = wait.Until(e => e.FindElement(mentorFirstName));
        }

        public EditMentorDetailsPage ClickEditMentorDetaisNav()
        {
            driver.FindElement(editDetailsNavLink).Click();
            return new EditMentorDetailsPage(driver);
        }

        public string[] GetTexFromAllFields()
        {
            return new string[]{
            driver.FindElement(mentorFirstName).Text,
            driver.FindElement(mentorLastName).Text,
            driver.FindElement(mentorEmail).Text
            };
        }
    }
}
