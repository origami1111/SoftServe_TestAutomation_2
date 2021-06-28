using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class EditSecretaryPage : BasePageWithHeaderSidebar
    {

        private By
                        firstNameLocator = By.XPath("//input[@name='firstName']"),
                        lastNameLocator = By.XPath("//input[@name='lastName']"),
                        emailLocator = By.XPath("//input[@name='email']"),
                        layOffButtonLocator = By.XPath("//button[text()='Lay off']"),
                        clearButtonLocator = By.XPath("//button[text()='Clear']"),
                        saveButtonLocator = By.XPath("//button[text()='Save']"),
                        deselectLabel = By.XPath("//label[text()='First Name:']"),
                        firstNameDangerField = By.XPath("//input[@name = 'firstName']/following-sibling::div"),
                        lastNameDangerField = By.XPath("//input[@name = 'lastName']/following-sibling::div"),
                        emailDangerField = By.XPath("//input[@name = 'email']/following-sibling::div");

        public EditSecretaryPage(IWebDriver driver) : base(driver)
        {

        }

        public string Fill_FirstName(string FirstName)
        {

            var firstNameField = driver.FindElement(firstNameLocator);

            firstNameField.SendKeys(Keys.LeftShift + Keys.Home);
            firstNameField.SendKeys(FirstName);
            ClickItem(deselectLabel);

            var dangerMessage = driver.FindElement(firstNameDangerField).Text;

            return dangerMessage;

        }

        public string Fill_LastName(string LastName)
        {

            var lastNameField = driver.FindElement(lastNameLocator);

            lastNameField.SendKeys(Keys.LeftShift + Keys.Home);
            lastNameField.SendKeys(LastName);
            ClickItem(deselectLabel);

            var dangerMessage = driver.FindElement(lastNameDangerField).Text;

            return dangerMessage;

        }

        public string Fill_Email(string Email)
        {

            var emailField = driver.FindElement(emailLocator);

            emailField.SendKeys(Keys.LeftShift + Keys.Home);
            emailField.SendKeys(Email);
            ClickItem(deselectLabel);

            var dangerMessage = driver.FindElement(emailDangerField).Text;

            return dangerMessage;

        }

    }
}
