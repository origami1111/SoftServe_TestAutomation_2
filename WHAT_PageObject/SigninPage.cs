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

        public SignInPage fillEmail(string email)
        {
            driver.FindElement(emailLocator).SendKeys(email);

            return this;
        }

        public SignInPage fillPassword(string password)
        {
            driver.FindElement(passwordLocator).SendKeys(password);

            return this;
        }

        //public HomePage enterLoginButton()
        //{
        //    driver.FindElement(signinButtonLocator).Click();

        //    return new HomePage(driver);
        //}

        public RegistrationPage clickRegistrationLink()
        {
            driver.FindElement(registrationLinkLocator).Click();

            return new RegistrationPage(driver);
        }

        //public ForgotPasswordPage clickForgotPasswordLink()
        //{
        //    driver.FindElement(forgotPasswordLinkLocator).Click();

        //    return new ForgotPasswordPage(driver);
        //}
    }
}
