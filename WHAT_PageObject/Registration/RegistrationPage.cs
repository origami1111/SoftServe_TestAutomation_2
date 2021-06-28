using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class RegistrationPage : BasePage
    {
        /// <summary>
        /// Locators
        /// </summary>
        private By firstNameField = By.Id("firstName");
        private By lastNameField = By.Id("lastName");
        private By emailField = By.Id("email");
        private By passwordField = By.Id("password");
        private By confirmPasswordField = By.Id("confirm-password");
        private By signUpButton = By.CssSelector("button[type='submit']");
        private By logInLink = By.CssSelector("a[href='/auth']");

        private By errorFirstName = By.XPath("//input[@name='firstName']//following-sibling::p");
        private By errorLastName = By.XPath("//input[@name='lastName']//following-sibling::p");
        private By errorEmail = By.XPath("//input[@name='email']//following-sibling::p");
        private By errorPassword = By.XPath("//input[@name='password']//following-sibling::p");
        private By errorConfirmPassword = By.XPath("//input[@name='confirmPassword']//following-sibling::p");

        public RegistrationPage(IWebDriver driver) : base(driver)
        {
        }

        public RegistrationPage FillFirstName(string firstName)
        {
            driver.FindElement(firstNameField).SendKeys(firstName);

            return this;
        }

        public RegistrationPage FillLastName(string lastName)
        {
            driver.FindElement(lastNameField).SendKeys(lastName);

            return this;
        }

        public RegistrationPage FillEmail(string email)
        {
            driver.FindElement(emailField).SendKeys(email);

            return this;
        }

        public RegistrationPage FillPassword(string password)
        {
            driver.FindElement(passwordField).SendKeys(password);

            return this;
        }

        public RegistrationPage FillConfirmPassword(string confirmPassword)
        {
            driver.FindElement(confirmPasswordField).SendKeys(confirmPassword);

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

        public string GetErrorMessageFirstName()
        {
            string text = driver.FindElement(errorFirstName).Text;

            return text;
        }

        public string GetErrorMessageLastName()
        {
            string text = driver.FindElement(errorLastName).Text;

            return text;
        }

        public string GetErrorMessageEmail()
        {
            string text = driver.FindElement(errorEmail).Text;

            return text;
        }

        public string GetErrorMessagePassword()
        {
            string text = driver.FindElement(errorPassword).Text;

            return text;
        }

        public string GetErrorMessageConfirmPassword()
        {
            string text = driver.FindElement(errorConfirmPassword).Text;

            return text;
        }
    }
}
