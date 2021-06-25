using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;

namespace WHAT_PageFactory
{
    public class SignInPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        [FindsBy(How = How.Id, Using = "email")]
        [CacheLookup]
        private IWebElement email;

        public SignInPage(IWebDriver driver) : base(driver)
        {
            string currentURL = driver.Url;
            if (!Equals(currentURL, "http://localhost:8080/auth"))
            {
                throw new Exception("This is not the 'Sign In' page");
            }
        }

        public SignInPage fillEmail(string email)
        {
            this.email.SendKeys(email);

            return this;
        }

        public SignInPage fillPassword(string password)
        {
            this.password.SendKeys(password);

            return this;
        }

        public void ClickSignInButton()
        {
            signInButton.Click();
        }

        public LessonsPage SignInAsMentor()
        {


            return new LessonsPage(driver);
        }

        // public RegistrationPage clickRegistrationLink()
        //   {
        //      driver.FindElement(registrationLinkLocator).Click();

        //return new RegistrationPage(driver);
        //   }

        //public ForgotPasswordPage clickForgotPasswordLink()
        //{
        //    driver.FindElement(forgotPasswordLinkLocator).Click();

        //    return new ForgotPasswordPage(driver);
        //}


    }
}
