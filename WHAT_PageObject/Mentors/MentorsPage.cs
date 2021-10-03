using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class MentorsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By disabledMentorsToggle = By.XPath("//input[@type ='checkbox']");
        private By searchField = By.XPath("//input[@type='text']");
        private By rowAmountDropdownMenu = By.XPath("//select");
        private By switchToCardViewButton = By.XPath("//*[contains(@href,'Card.svg')]/parent::*/parent::button");
        private By switchToTableViewButton = By.XPath("//*[contains(@href,'List.svg')]/parent::*/parent::button");
        private By addMentorButton = By.XPath("//span[text()='Add a mentor']/parent::button");

        private By previousPageTopButton = By.XPath("//h2[text()='Mentors']/parent::div//button[text()='<']");
        private By nextPageTopButton = By.XPath("//h2[text()='Mentors']/parent::div//button[text()='>']");

        private By previousPageBottomButton = By.XPath("//div[@class='row mr-0']//button[text()='<']");
        private By nextPageBottomButton = By.XPath("//div[@class='row mr-0']//button[text()='>']");

        private By sortingRowFirstName = By.XPath("//span[@data-sorting-param='firstName']");
        private By sortingRowLastName = By.XPath("//span[@data-sorting-param='lastName']");
        private By sortingRowEmail = By.XPath("//span[@data-sorting-param='email']");

        private By mentorsTable = By.XPath("//table");

        private By mentorsCount = By.XPath("//span[text()[4]=' mentors']");
        #endregion

        private By mentorFirstName(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[1]");
        private By mentorLastName(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[2]");
        private By mentorEmail(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[3]");
        private By editMentorButton(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[4]");

        private By pageTopButton(int pageNumber) => By.XPath($"//h2[text()='Mentors']/parent::div//button[text()='{pageNumber}']");
        private By pageBottomButton(int pageNumber) => By.XPath($"//div[@class='row mr-0']//button[text()='{pageNumber}']");

        public MentorsPage(IWebDriver driver) : base(driver)
        {
            
        }

        private int GetDisplayedMentorCount()
        {
            string mentorCountElement = driver.FindElement(mentorsCount).Text;
            string[] countComponents = mentorCountElement.Split(" ");
            int mentorCount;
            int.TryParse(countComponents[0], out mentorCount);
            return mentorCount;
        }

        private int GetTotalMentorCount()
        {
            string mentorCountElement = driver.FindElement(mentorsCount).Text;
            string[] countComponents = mentorCountElement.Split(" ");
            int mentorCount;
            int.TryParse(countComponents[2], out mentorCount);
            return mentorCount;
        }

        private void WaitMentorsLoad()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            IWebElement firstResult = wait.Until(e => e.FindElement(mentorsTable));
        }

        public MentorsPage FillSearchField(string searchText)
        {
            WaitMentorsLoad();
            FillField(searchField, searchText);
            return this;
        }

        public MentorsPage ClickDisabledMentorsToggle()
        {
            //ClickItem(disabledMentorsToggle);
            //TODO: possibly swap ClickItem code with this:
            IWebElement element = driver.FindElement(disabledMentorsToggle);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Click().Build().Perform();
            return this;
        }

        public MentorsPage ClickSwitchToCardViewButton()
        {
            ClickItem(switchToCardViewButton);
            return this;
        }

        public MentorsPage ClickSwitchToTableViewButton()
        {
            ClickItem(switchToTableViewButton);
            return this;
        }

        public UnassignedUsersPage ClickAddMentorButton()
        {
            ClickItem(addMentorButton);
            return new UnassignedUsersPage(driver);
        }

        public MentorsPage ClickPreviousPageTopButton()
        {
            ClickItem(previousPageTopButton);
            return this;
        }

        public MentorsPage ClickNextPageTopButton()
        {
            ClickItem(nextPageTopButton);
            return this;
        }

        public MentorsPage ClickPreviousPageBottomButton()
        {
            ClickItem(previousPageBottomButton);
            return this;
        }

        public MentorsPage ClickNextPageBottomButton()
        {
            ClickItem(nextPageBottomButton);
            return this;
        }

        public MentorsPage ClickSortByFirstName()
        {
            ClickItem(sortingRowFirstName);
            return this;
        }

        public MentorsPage ClickSortByLastName()
        {
            ClickItem(sortingRowLastName);
            return this;
        }

        public MentorsPage ClickSortByEmail()
        {
            ClickItem(sortingRowEmail);
            return this;
        }

        public MentorsPage WaitUntilMentorsTableLoads()
        {
            WaitMentorsLoad();
            return this;
        }

        public IWebElement verifyMentorsTableExists()
        {
            IWebElement table = null;
            try
            {
                table = driver.FindElement(mentorsTable);
            }
            catch
            {

            }
            return table;
        }
    }
}
