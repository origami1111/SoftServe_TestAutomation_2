using OpenQA.Selenium;
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
        private By forgotPasswordLinkLocator = By.CssSelector("a[href='/forgot-password']");
        private By registrationLinkLocator = By.CssSelector("a[href='/registration']");

        public SignInPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;

            string currentURL = driver.Url;
            if (!Equals(currentURL, "http://localhost:8080/auth"))
            {
                throw new Exception("This is not the 'Log In' page");
            }
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
            driver.FindElement(signInButtonLocator).Click();
        }

        public void ClickRegistrationLink()
        {
            driver.FindElement(registrationLinkLocator).Click();

        }

    }
}
