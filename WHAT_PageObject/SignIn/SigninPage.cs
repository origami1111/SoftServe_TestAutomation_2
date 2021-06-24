﻿using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class SignInPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        private By emailField = By.Id("email");
        private By passwordField = By.Id("password");
        private By signInButton = By.CssSelector("button[type='submit']");
        private By registrationLink = By.CssSelector("a[href='/registration']");
        private By errorText = By.XPath("//p[contains(.,'An error occurred')]");


        public SignInPage(IWebDriver driver) : base(driver)
        {
        }

        public SignInPage FillEmail(string email)
        {
            driver.FindElement(emailField).SendKeys(email);

            return this;
        }

        public SignInPage FillPassword(string password)
        {
            driver.FindElement(passwordField).SendKeys(password);

            return this;
        }

        public void ClickSignInButton()
        {
            driver.FindElement(signInButton).Click();
        }

        public RegistrationPage ClickRegistrationLink()
        {
            driver.FindElement(registrationLink).Click();
            
            return new RegistrationPage(driver);
        }

        public string GetAnErrorOccured()
        {
            return driver.FindElement(errorText).Text;
        }
    }
}

