﻿using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class SignInPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        private By emailLocator = By.Id("email");
        private By passwordLocator = By.Id("password");
        private By signinButtonLocator = By.CssSelector("button[type='submit']");
        private By forgotPasswordLinkLocator = By.CssSelector("a[href='/forgot-password']");
        private By registrationLinkLocator = By.CssSelector("a[href='/registration']");

        public SignInPage(IWebDriver driver) : base(driver)
        {
            
        }

        public SignInPage FillEmail(string email)
        {
            driver.FindElement(emailLocator).SendKeys(email);

            return this;
        }

        public SignInPage FillPassword(string password)
        {
            driver.FindElement(passwordLocator).SendKeys(password);

            return this;
        }

        public void ClickLoginButton()
        {
            driver.FindElement(signinButtonLocator).Click();
        }

        public RegistrationPage ClickRegistrationLink()
        {
            driver.FindElement(registrationLinkLocator).Click();

            return new RegistrationPage(driver);
        }

    }
}
