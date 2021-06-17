using OpenQA.Selenium;

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

        //public RegistrationPage clickRegistrationLink()
        //{
        //    driver.FindElement(registrationLinkLocator).Click();

        //    return new RegistrationPage(driver);
        //}

        //public ForgotPasswordPage clickForgotPasswordLink()
        //{
        //    driver.FindElement(forgotPasswordLinkLocator).Click();

        //    return new ForgotPasswordPage(driver);
        //}
    }
}
