using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class EditSecretaryPage : BasePageWithHeaderSidebar
    {
        #region Locators
        private By
                       firstName = By.XPath("//input[@name='firstName']"),
                       lastName = By.XPath("//input[@name='lastName']"),
                       email = By.XPath("//input[@name='email']"),
                       deselectLabel = By.XPath("//label[text()='First Name:']"),
                       firstNameDangerField = By.XPath("//input[@name = 'firstName']/following-sibling::div"),
                       lastNameDangerField = By.XPath("//input[@name = 'lastName']/following-sibling::div"),
                       emailDangerField = By.XPath("//input[@name = 'email']/following-sibling::div");

        public By
                       layOff = By.XPath("//button[text()='Lay off']"),
                       clear = By.XPath("//button[text()='Clear']"),
                       save = By.XPath("//button[text()='Save']");
        #endregion


        public EditSecretaryPage(IWebDriver driver) : base(driver)
        {

        }

        public EditSecretaryPage Fill_FirstName(string FirstName)
        {
            var firstNameField = driver.FindElement(firstName);

            firstNameField.SendKeys(Keys.LeftShift + Keys.Home);
            firstNameField.SendKeys(FirstName);
            ClickItem(deselectLabel);

            return this;
        }

        public EditSecretaryPage Fill_LastName(string LastName)
        {
            var lastNameField = driver.FindElement(lastName);

            lastNameField.SendKeys(Keys.LeftShift + Keys.Home);
            lastNameField.SendKeys(LastName);
            ClickItem(deselectLabel);

            return this;
        }

        public EditSecretaryPage Fill_Email(string Email)
        {
            var emailField = driver.FindElement(email);

            emailField.SendKeys(Keys.LeftShift + Keys.Home);
            emailField.SendKeys(Email);
            ClickItem(deselectLabel);

            return this;
        }

        public string GetFirstNameDangerField()
        {
            return driver.FindElement(firstNameDangerField).Text;
        }
        public string GetLastNameDangerField()
        {
            return driver.FindElement(lastNameDangerField).Text;
        }
        public string GetEmailDangerField()
        {
            return driver.FindElement(emailDangerField).Text;
        }

        public EditSecretaryPage ClickSaveButton()
        {
            ClickItem(save);

            return this;
        }

        public EditSecretaryPage ClickClearButton()
        {
            ClickItem(clear);

            return this;
        }

        public string GetFirstName()
        {
            return driver.FindElement(firstName).GetAttribute("value");
        }
        public string GetLastName()
        {
            return driver.FindElement(lastName).GetAttribute("value");
        }
        public string GetEmail()
        {
            return driver.FindElement(email).GetAttribute("value");
        }
    }
}
