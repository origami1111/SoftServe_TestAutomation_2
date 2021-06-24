using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class RegistrationPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        private By firstName = By.Id("firstName");
        private By lastName = By.Id("lastName");
        private By email = By.Id("email");
        private By password = By.Id("password");
        private By confirmPassword = By.Id("confirm-password");
        private By signUpButton = By.CssSelector("button[type='submit']");
        private By logInLink = By.XPath("a[href='/auth']");

        public RegistrationPage(IWebDriver driver) : base(driver)
        {
        }

        public RegistrationPage FillFirstName(string firstName)
        {
            driver.FindElement(this.firstName).SendKeys(firstName);

            return this;
        }

        public RegistrationPage FillLastName(string lastName)
        {
            driver.FindElement(this.lastName).SendKeys(lastName);

            return this;
        }

        public RegistrationPage FillEmail(string email)
        {
            driver.FindElement(this.email).SendKeys(email);

            return this;
        }

        public RegistrationPage FillPassword(string password)
        {
            driver.FindElement(this.password).SendKeys(password);

            return this;
        }

        public RegistrationPage FillConfirmPassword(string confirmPassword)
        {
            driver.FindElement(this.confirmPassword).SendKeys(confirmPassword);

            return this;
        }

        public RegistrationPage ClickSignUpButton()
        {
            driver.FindElement(signUpButton).Click();

            return this;
        }

        public SignInPage ClickLogInLink()
        {
            driver.FindElement(logInLink).Click();

            return new SignInPage(driver);
        }

    }
}
