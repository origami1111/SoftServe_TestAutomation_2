using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class ChangePasswordTests
    {

        private IWebDriver driver;

        private ChangePasswordPage changePasswordPage;

        string currentPass = "What_123";
        string newPass = "What_1234";

        [OneTimeSetUp]
        public void Setup()
        {

            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:8080/");

        }
        [SetUp]
        public void SetupPage()
        {
           
            changePasswordPage = new SignInPage(driver)
                .SignInAsMentor("mentor@gmail.com", currentPass).ClickChangePassword();

        }
        [Test]
        public void ChangePasswordTest()
        {
            
            changePasswordPage
                .FillCurrentPassword(currentPass)
                .FillNewPassword(newPass)
                .FillConfirmNewPassword(newPass)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu();

        }
        [TearDown]
        public void Logout()
        {
            changePasswordPage
                .ClickChangePassword()
                .FillCurrentPassword(newPass)
                .FillNewPassword(currentPass)
                .FillConfirmNewPassword(currentPass)
                .ClickSaveButton()
                .ClickSaveInPopUpMenu();

        }
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

    }
}
