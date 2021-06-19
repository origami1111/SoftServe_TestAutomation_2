﻿using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class SignInPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        private By emailLocator = By.Id("email");
        private By passwordLocator = By.Id("password");
        private By signInButtonLocator = By.CssSelector("button[type='submit']");
        private By registrationLinkLocator = By.CssSelector("a[href='/registration']");

        public SignInPage(IWebDriver driver) : base(driver)
        {
            string currentURL = driver.Url;
            if (!Equals(currentURL, "http://localhost:8080/auth"))
            {
                throw new Exception("This is not the 'Log In' page");
            }
        }

        private SignInPage FillEmail(string email)
        {
            driver.FindElement(emailLocator).SendKeys(email);

            return this;
        }

        private SignInPage FillPassword(string password)
        {
            driver.FindElement(passwordLocator).SendKeys(password);

            return this;
        }

        private void ClickSignInButton()
        {
            driver.FindElement(signInButtonLocator).Click();
        }

        public RegistrationPage ClickRegistrationLink()
        {
            driver.FindElement(registrationLinkLocator).Click();
            
            return new RegistrationPage(driver);
        }

        public LessonsPage SignInAsMentor(string email, string password)
        {
            FillEmail(email);
            FillPassword(password);
            ClickSignInButton();

            return new LessonsPage(driver);
        }

        public MentorsPage SignInAsSercetar(string email, string password)
        {
            FillEmail(email);
            FillPassword(password);
            ClickSignInButton();

            return new MentorsPage(driver);
        }

        public SupportPage SignInAsStudent(string email, string password)
        {
            FillEmail(email);
            FillPassword(password);
            ClickSignInButton();

            return new SupportPage(driver);
        }

    }
}
