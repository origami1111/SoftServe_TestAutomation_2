using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class ChangePasswordPage : BasePageWithHeaderSidebar
    {
        private By email = By.Id("email");
        private By currentPassword = By.Id("currentPassword");
        private By newPassword = By.Id("newPassword");
        private By confirmNewPassword = By.Id("confirmNewPassword");
        private By saveButton = By.XPath("//button[contains(.,\'Save\')]");
        private By cancelButton = By.XPath("//button[contains(.,\'Cancel\')]");
        private By cancelInPopUpMenu = By.CssSelector(".btn-secondary");
        private By saveInPopUpMenu = By.XPath("//button[contains(.,'Confirm')]");
        private By currentPassErrorField = By.XPath("//input[@name='currentPassword']//following-sibling::div");
        private By newPassErrorField = By.XPath("//input[@name='newPassword']//following-sibling::div");
        private By confirmPassErrorField = By.XPath("//input[@name='confirmNewPassword']//following-sibling::div");
        private By passSuccessMessage = By.CssSelector(".fade");

        public ChangePasswordPage(IWebDriver driver) : base(driver)
        {

        }

        public ChangePasswordPage FillCurrentPassword(string currentPass)
        {
            FillField(currentPassword, currentPass);
            return this;
        }

        public ChangePasswordPage FillNewPassword(string newPass)
        {
            FillField(newPassword, newPass);
            return this;
        }

        public ChangePasswordPage FillConfirmNewPassword(string newPass)
        {
            FillField(confirmNewPassword, newPass);
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

        public string VerifyCurrentEmail()
        {
            return driver.FindElement(email).GetAttribute("value");
        }

        public string VerifySuccesMessage()
        {
            return driver.FindElement(passSuccessMessage).Text;
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
    }
}
