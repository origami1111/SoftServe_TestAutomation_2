using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class ChangePasswordPage : Sidebar
    {
        private By email = By.Id("email");
        private By email1 = By.XPath("//*[@id='email']");

        private By currentPassword = By.Id("currentPassword");
        private By newPassword = By.Id("newPassword");
        private By confirmNewPassword = By.Id("confirmNewPassword");
        private By saveButton = By.XPath("//button[contains(.,\'Save\')]");
        private By cancelButton = By.XPath("//button[contains(.,\'Cancel\')]");
        private By cancelInPopUpMenu = By.CssSelector(".btn-secondary");
        private By saveInPopUpMenu = By.XPath("//button[contains(.,'Confirm')]");

        private By header = By.XPath("//span[contains(.,'▼')]");
        private By changePassword = By.LinkText("Change password");

        private By currentPassErrorField = By.XPath("//input[@name='currentPassword']//following-sibling::div");
        private By newPassErrorField = By.XPath("//input[@name='newPassword']//following-sibling::div");
        private By confirmPassErrorField = By.XPath("//input[@name='confirmNewPassword']//following-sibling::div");

        private By passSuccessMessage = By.XPath("//div[@role='alert']");
        private By passSuccessMessage1 = By.CssSelector(".fade");
        public ChangePasswordPage(IWebDriver driver) : base(driver)
        {

        }

        public string VerifyCurrentEmail() 
        {
            return driver.FindElement(email).GetAttribute("value");
        }
        public string VerifySuccesMessage() 
        { 
            return driver.FindElement(passSuccessMessage1).Text;
        }
        public string VerifyErrorMassegeForCurrentPassword()
        {
            return driver.FindElement(currentPassErrorField).Text;
        }

        public string VerifyErrorMassegeForNewPassword()
        {
            return driver.FindElement(newPassErrorField).Text;
        }

        public string VerifyErrorMassegeForConfirmPassword()
        {
            return driver.FindElement(confirmPassErrorField).Text;
        }
        public ChangePasswordPage FillCurrentPassword(string currentPassword)
        {
            FillField(this.currentPassword, currentPassword);
            return this;
        }

        public ChangePasswordPage FillNewPassword(string newPassword)
        {
            FillField(this.newPassword, newPassword);
            return this;
        }

        public ChangePasswordPage FillConfirmNewPassword(string newPassword)
        {
            FillField(confirmNewPassword, newPassword);
            return this;
        }

        public ChangePasswordPage ClickSaveButton()
        {
            ClickItem(saveButton);
            return this;
        }

        public ChangePasswordPage ClickCancelButton()
        {
            ClickItem(cancelButton);
            return this;
        }

        public ChangePasswordPage ClickCancelButtonInPopUpMenu()
        {
            ClickItem(cancelInPopUpMenu);
            return this;
        }

        public ChangePasswordPage ClickSaveInPopUpMenu()
        {
            ClickItem(saveInPopUpMenu);
            return this;
        }
        public ChangePasswordPage ClickChangePassword()
        {
            ClickItem(header);
            ClickItem(changePassword);
            return new ChangePasswordPage(driver);
        }
    }
}
