using OpenQA.Selenium;
using System;

namespace WHAT_PageObject
{
    public class ChangePasswordPage : Sidebar
    {
        private By emailLocator = By.Id("email");
        private By currentPasswordLocator = By.Id("currentPassword");
        private By newPasswordLocator = By.Id("newPassword");
        private By confirmNewPasswordLocator = By.Id("confirmNewPassword");
        private By saveButtonLocator = By.XPath("//button[contains(.,\'Save\')]");
        private By cancelButtonLocator = By.XPath("//button[contains(.,\'Cancel\')]");
        private By cancelInPopUpMenuLocator = By.XPath("//div[3]/button");
        private By saveButtonInPopUpMenu = By.XPath("//button[contains(.,'Confirm')]");

        public ChangePasswordPage(IWebDriver driver) : base(driver)
        {

        }
        
        public ChangePasswordPage FillCurrentPassword(string currentPassword)
        {
            FillField(currentPasswordLocator, currentPassword);
            return this;
        }

        public ChangePasswordPage FillNewPassword(string newPassword)
        {
            FillField(newPasswordLocator, newPassword);
            return this;
        }

        public ChangePasswordPage FillConfirmNewPassword(string newPassword)
        {
            FillField(confirmNewPasswordLocator, newPassword);
            return this;
        }

        public ChangePasswordPage ClickSaveButton()
        {
            ClickItem(saveButtonLocator);
            return this;
        }

        public ChangePasswordPage ClickCancelButton()
        {
            ClickItem(cancelButtonLocator);
            return this;
        }

        public ChangePasswordPage ClickCancelButtonInPopUpMenu()
        {
            ClickItem(cancelInPopUpMenuLocator);
            return this;
        }

        public ChangePasswordPage ClickSaveInPopUpMenu()
        {
            ClickItem(saveButtonInPopUpMenu);
            return this;
        }
        public ChangePasswordPage ClickChangePassword()
        {
            
            ClickItem(By.CssSelector(".header__header__dropdown-icon___1CTJ8"));
            ClickItem(By.LinkText("Change password"));

            return new ChangePasswordPage(driver);
        }
    }
}
