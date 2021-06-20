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

        public void enterLoginButton()
        {
            driver.FindElement(signInButtonLocator).Click();
        }


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

        public LessonsPage SignInAsMentor()
        {
             fillEmail("mentor@gmail.com")
            .fillPassword("What_123")
            .enterLoginButton();

            return new LessonsPage(driver);
        }
    }
}
