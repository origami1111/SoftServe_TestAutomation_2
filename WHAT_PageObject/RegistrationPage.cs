using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class RegistrationPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        private By firstNameLocator = By.Id("firstName");
        private By lastNameLocator = By.Id("lastName");
        private By emailLocator = By.Id("email");
        private By passwordLocator = By.Id("password");
        private By confirmPasswordLocator = By.Id("confirm-password");
        private By signUpButtonLocator = By.CssSelector("button[type='submit']");
        private By logInLinkLocator = By.XPath("a[href='/auth']");

        public RegistrationPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;

            string currentURL = driver.Url;
            if (!Equals(currentURL, "http://localhost:8080/registration"))
            {
                throw new Exception("This is not the 'Registration' page");
            }
        }

        public RegistrationPage fillFirstName(string firstName)
        {
            driver.FindElement(firstNameLocator).SendKeys(firstName);

            return this;
        }

        public RegistrationPage fillLastName(string lastName)
        {
            driver.FindElement(lastNameLocator).SendKeys(lastName);

            return this;
        }

        public RegistrationPage fillEmail(string email)
        {
            driver.FindElement(emailLocator).SendKeys(email);

            return this;
        }

        public RegistrationPage fillPassword(string password)
        {
            driver.FindElement(passwordLocator).SendKeys(password);

            return this;
        }

        public RegistrationPage fillConfirmPassword(string confirmPassword)
        {
            driver.FindElement(confirmPasswordLocator).SendKeys(confirmPassword);

            return this;
        }

        public RegistrationPage clickSignUpButton()
        {
            driver.FindElement(signUpButtonLocator).Click();

            return this;
        }

        public SignInPage clickLogInLink()
        {
            driver.FindElement(logInLinkLocator).Click();

            return new SignInPage(driver);
        }


    }
}
