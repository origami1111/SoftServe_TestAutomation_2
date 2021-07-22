using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class EditStudentDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By detailsNavLink = By.LinkText("Student details");
        private By firstName = By.XPath("//input[@name='firstName']");
        private By lastName = By.XPath("//input[@name='lastName']");
        private By email = By.XPath("//input[@name='email']");
        private By deselectLabel = By.XPath("//label[text()='First Name:']");
        private By firstNameDangerField = By.XPath("//input[@name = 'firstName']/following-sibling::div");
        private By lastNameDangerField = By.XPath("//input[@name = 'lastName']/following-sibling::div");
        private By emailDangerField = By.XPath("//input[@name = 'email']/following-sibling::div");
        private By clearButton = By.XPath("//button[@class='w-100 btn btn-secondary edit-students-details__button___WOMG6']");
        private By saveButton = By.XPath("//button[@class='w-100 btn btn-info edit-students-details__button___WOMG6']");
        private By container = By.XPath("//div[@class='container']");
        #endregion
        public EditStudentDetailsPage(IWebDriver driver) : base(driver)
        {

        }

        private EditStudentDetailsPage DeleteTextWithBackspaces(int backspacesNumber, By locator)
        {
            var field = driver.FindElement(locator);
            for (int i = 0; i < backspacesNumber; i++)
            {
                field.SendKeys(Keys.Backspace);
            }
            return this;
        }

        public EditStudentDetailsPage FillEmail(string emailMessage)
        {
            var emailField = driver.FindElement(email);
            this.DeleteTextWithBackspaces(GetEmail().Length, email);
            emailField.SendKeys(emailMessage);
            ClickItem(deselectLabel);
            return this;
        }

        public EditStudentDetailsPage FillFirstName(string firstNameMessage)
        {
            var firstNameField = driver.FindElement(firstName);
            this.DeleteTextWithBackspaces(GetFirstName().Length, firstName);
            firstNameField.SendKeys(firstNameMessage);
            ClickItem(deselectLabel);
            return this;
        }

        public EditStudentDetailsPage FillLastName(string lastNameMessage)
        {
            var lastNameField = driver.FindElement(lastName);
            this.DeleteTextWithBackspaces(GetEmail().Length, lastName);
            lastNameField.SendKeys(lastNameMessage);
            ClickItem(deselectLabel);
            return this;
        }

        public StudentDetailsPage ClickEditStudentsDetaisNav()
        {
            driver.FindElement(detailsNavLink).Click();
            return new StudentDetailsPage(driver);
        }

        public EditStudentDetailsPage ClickClearButton()
        {
            driver.FindElement(clearButton).Click();
            return this;
        }

        public StudentsPage ClickSaveButton()
        {
            driver.FindElement(saveButton).Click();
            return new StudentsPage(driver);
        }

        public EditStudentDetailsPage WaitStudentsEditingLoad()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            IWebElement firstResult = wait.Until(e => e.FindElement(container));
            return this;
        }

        public string GetErrorMessageFirstName()
        {
            return driver.FindElement(firstNameDangerField).Text;
        }

        public string GetErrorMessageLastName()
        {
            return driver.FindElement(lastNameDangerField).Text;
        }

        public string GetErrorMessageEmail()
        {
            try
            {
                driver.FindElement(emailDangerField);
                return driver.FindElement(emailDangerField).Text;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetFirstName()
        {
            return driver.FindElement(firstName).GetAttribute("value");
        }

        private string GetLastName()
        {
            return driver.FindElement(lastName).GetAttribute("value");
        }

        private string GetEmail()
        {
            return driver.FindElement(email).GetAttribute("value");
        }
    }
}
