using System;
using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class ChangePassword : BasePage
    {
        private By emailLocator = By.Id("email");
        private By currentPasswordLocator = By.Id("currentPassword");
        private By newPasswordLocator = By.Id("newPassword");
        private By confirmNewPasswordLocator = By.Id("confirmNewPassword");
        private By saveButtonLocator = By.XPath("//button[contains(.,\'Save\')]");
        private By cancelButtonLocator = By.XPath("//button[contains(.,\'Cancel\')]");
        private By cancelInPopUpMenuLocator = By.XPath("//div[3]/button");
        private By saveButtonInPopUpMenu = By.XPath("//button[contains(.,'Confirm')]");

        public ChangePassword(IWebDriver driver) : base(driver)
        {
            this.driver = driver;

            string currentURL = driver.Url;
            if (!Equals(currentURL, "http://localhost:8080/change-password"))
            {
                throw new Exception("This is not the 'Change-password' page");
            }
        }

        
        private void Fill(By locator, string text)
        {
            driver.FindElement(locator).Click();
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(text);
        }

        private void Click(By locator)
        {
            driver.FindElement(locator).Click();
        }

        public ChangePassword FillCurrentPassword(string currentPassword)
        {
            Fill(currentPasswordLocator,currentPassword);
            return this;
        }

        public ChangePassword FillNewPassword(string newPassword)
        {
            Fill(newPasswordLocator,newPassword);
            return this;
        }

        public ChangePassword FillConfirmNewPassword(string newPassword)
        {
            Fill(confirmNewPasswordLocator, newPassword);
            return this;
        }

        public ChangePassword ClickSaveButton()
        {
            Click(saveButtonLocator);
            return this;
        }

        public ChangePassword ClickCancelButton()
        {
            Click(cancelButtonLocator);
            return this;
        }

        public ChangePassword ClickCancelButtonInPopUpMenu()
        {
            Click(cancelInPopUpMenuLocator);
            return this;
        }

        public ChangePassword ClickSaveInPopUpMenu()
        {
            Click(saveButtonInPopUpMenu);
            return this;
        }
    }
}
