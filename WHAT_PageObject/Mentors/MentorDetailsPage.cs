using NUnit.Framework;
using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class MentorDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By editDetailsNavLink = By.LinkText(Locators.MentorDetailsPage.EDIT_MENTOR_LINK);
        private By mentorFirstName = By.XPath(Locators.MentorDetailsPage.FIRST_NAME);
        private By mentorLastName = By.XPath(Locators.MentorDetailsPage.LAST_NAME);
        private By mentorEmail = By.XPath(Locators.MentorDetailsPage.EMAIL);
        private By mentorDetails = By.XPath(Locators.MentorDetailsPage.MENTOR_DETAILS);
        #endregion

        public MentorDetailsPage(IWebDriver driver) : base(driver)
        {

        }

        public EditMentorDetailsPage ClickEditMentorDetaisNav()
        {
            driver.FindElement(editDetailsNavLink).Click();
            return new EditMentorDetailsPage(driver);
        }

        private string[] GetTexFromAllFields()
        {
            return new string[] {
                driver.FindElement(mentorFirstName).Text,
                driver.FindElement(mentorLastName).Text,
                driver.FindElement(mentorEmail).Text
            };
        }

        public MentorDetailsPage WaitUntilMentorDetailsLoads()
        {
            WaitUntilElementLoads<MentorDetailsPage>(mentorDetails);
            return this;
        }

        public MentorDetailsPage VerifyFirstName(string expected)
        {
            string actual = GetFirstName();
            Assert.AreEqual(expected, actual);
            return this;
        }

        public MentorDetailsPage VerifyLastName(string expected)
        {
            string actual = GetLastName();
            Assert.AreEqual(expected, actual);
            return this;
        }

        public MentorDetailsPage VerifyEmail(string expected)
        {
            string actual = GetEmail();
            Assert.AreEqual(expected, actual);
            return this;
        }

        public string GetFirstName()
        {
            return driver.FindElement(mentorFirstName).Text;
        }

        public string GetLastName()
        {
            return driver.FindElement(mentorLastName).Text;
        }

        public string GetEmail()
        {
            return driver.FindElement(mentorEmail).Text;
        }
    }
}
