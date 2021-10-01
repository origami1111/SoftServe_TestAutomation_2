using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class MentorDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By editDetailsNavLink = By.LinkText("Edit a mentor");
        private By mentorFirstName = By.XPath($"//span[@data-testid='firstName']");
        private By mentorLastName = By.XPath($"//span[@data-testid='lastName']");
        private By mentorEmail = By.XPath($"//span[@data-testid='email']");
        #endregion

        public MentorDetailsPage(IWebDriver driver) : base(driver)
        {

        }
        public EditMentorDetailsPage ClickEditStudentsDetaisNav()
        {
            driver.FindElement(editDetailsNavLink).Click();
            return new EditMentorDetailsPage(driver);
        }

        public string[] GetTexFromAllFields()
        {
            return new string[]{
            driver.FindElement(mentorFirstName).Text,
            driver.FindElement(mentorLastName).Text,
            driver.FindElement(mentorEmail).Text
            };
        }
    }
}
