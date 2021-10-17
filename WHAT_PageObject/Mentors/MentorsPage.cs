using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class MentorsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By disabledMentorsToggle = By.XPath(Locators.MentorsPage.DISABLED_MENTORS_TOGGLE);
        private By searchField = By.XPath(Locators.MentorsPage.SEARCH_FIELD);
        private By rowAmountDropdownMenu = By.XPath(Locators.MentorsPage.ROW_AMOUNT_DROPDOWN);
        private By switchToCardViewButton = By.XPath(Locators.MentorsPage.SWITCH_TO_CARDS);
        private By switchToTableViewButton = By.XPath(Locators.MentorsPage.SWITCH_TO_TABLE);
        private By addMentorButton = By.XPath(Locators.MentorsPage.ADD_MENTOR_BUTTON);

        private By previousPageTopButton = By.XPath(Locators.MentorsPage.PREVIOUS_PAGE_TOP_BUTTON);
        private By nextPageTopButton = By.XPath(Locators.MentorsPage.NEXT_PAGE_TOP_BUTTON);

        private By previousPageBottomButton = By.XPath(Locators.MentorsPage.PREVIOUS_PAGE_BOTTOM_BUTTON);
        private By nextPageBottomButton = By.XPath(Locators.MentorsPage.NEXT_PAGE_BOTTOM_BUTTON);

        private By sortingRowFirstName = By.XPath(Locators.MentorsPage.SORTING_FIRST_NAME);
        private By sortingRowLastName = By.XPath(Locators.MentorsPage.SORTING_LAST_NAME);
        private By sortingRowEmail = By.XPath(Locators.MentorsPage.SORTING_EMAIL);

        private By mentorsTable = By.XPath(Locators.MentorsPage.MENTORS_TABLE);
        private By mentorsCount = By.XPath(Locators.MentorsPage.MENTORS_COUNT);

        private By allFirstNames = By.XPath(Locators.MentorsPage.ALL_FIRST_NAMES);
        private By allLastNames = By.XPath(Locators.MentorsPage.ALL_LAST_NAMES);
        private By allEmails = By.XPath(Locators.MentorsPage.ALL_EMAILS);
        #endregion

        static public By mentorFirstName(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[1]");
        static public By mentorLastName(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[2]");
        static public By mentorEmail(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[3]");
        static public By editMentorButton(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[4]");

        static public By pageTopButton(int pageNumber) => By.XPath($"//h2[text()='Mentors']/parent::div//button[text()='{pageNumber}']");
        static public By pageBottomButton(int pageNumber) => By.XPath($"//div[@class='row mr-0']//button[text()='{pageNumber}']");

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

        private List<string> GetFirstNames()
        {
            var elements = driver.FindElements(allFirstNames);
            List<string> firstNameList = new List<string>();
            foreach (IWebElement element in elements)
            {
                firstNameList.Add(element.Text);
            }
            return firstNameList;
        }
        private List<string> GetLastNames()
        {
            var elements = driver.FindElements(allLastNames);
            List<string> lastNameList = new List<string>();
            foreach (IWebElement element in elements)
            {
                lastNameList.Add(element.Text);
            }
            return lastNameList;
        }
        private List<string> GetEmails()
        {
            var elements = driver.FindElements(allEmails);
            List<string> emailsList = new List<string>();
            foreach (IWebElement element in elements)
            {
                emailsList.Add(element.Text);
            }
            return emailsList;
        }

        public void WaitUntilElementLoad(By element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            wait.Until(e => e.FindElement(element));
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

        public EditMentorDetailsPage ClickEditMentorButtonOnRow(int row)
        {
            IWebElement element = driver.FindElement(editMentorButton(row));
            element.Click();
            return new EditMentorDetailsPage(driver);
        }

        public MentorsPage ClickDisabledMentorsToggle()
        {
            ActionClickItem(disabledMentorsToggle);
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

        public MentorsPage SelectFromRowAmountDropdown(string value)
        {
            var dropDownMenu = driver.FindElement(rowAmountDropdownMenu);
            var selectElement = new SelectElement(dropDownMenu);
            selectElement.SelectByText(value);
            return this;
        }

        public MentorsPage WaitUntilMentorsTableLoads()
        {
            WaitUntilElementLoads<MentorsPage>(mentorsTable);
            return this;
        }

        public MentorsPage VerifyCorrectSorftingByFirstNameAsc()
        {
            List<string> firstNamesSortedByFrontEnd = GetFirstNames();
            List<string> firstNamesSortedByTest = new List<string>(firstNamesSortedByFrontEnd);
            firstNamesSortedByTest.Sort(StringComparer.Ordinal);
            CollectionAssert.AreEqual(firstNamesSortedByTest, firstNamesSortedByFrontEnd);
            return this;
        }
        public MentorsPage VerifyCorrectSorftingByFirstNameDesc()
        {
            List<string> firstNamesSortedByFrontEnd = GetFirstNames();
            List<string> firstNamesSortedByTest = new List<string>(firstNamesSortedByFrontEnd);
            firstNamesSortedByTest.Sort(StringComparer.Ordinal);
            firstNamesSortedByTest.Reverse();
            CollectionAssert.AreEqual(firstNamesSortedByTest, firstNamesSortedByFrontEnd);
            return this;
        }
        public MentorsPage VerifyCorrectSorftingByLastNameAsc()
        {
            List<string> lastNamesSortedByFrontEnd = GetLastNames();
            List<string> lastNamesSortedByTest = new List<string>(lastNamesSortedByFrontEnd);
            lastNamesSortedByTest.Sort(StringComparer.Ordinal);
            CollectionAssert.AreEqual(lastNamesSortedByTest, lastNamesSortedByFrontEnd);
            return this;
        }
        public MentorsPage VerifyCorrectSorftingByLastNameDesc()
        {
            List<string> lastNamesSortedByFrontEnd = GetLastNames();
            List<string> lastNamesSortedByTest = new List<string>(lastNamesSortedByFrontEnd);
            lastNamesSortedByTest.Sort(StringComparer.Ordinal);
            lastNamesSortedByTest.Reverse();
            CollectionAssert.AreEqual(lastNamesSortedByTest, lastNamesSortedByFrontEnd);
            return this;
        }
        public MentorsPage VerifyCorrectSorftingByEmailAsc()
        {
            List<string> emailsSortedByFrontEnd = GetEmails();
            List<string> emailsSortedByTest = new List<string>(emailsSortedByFrontEnd);
            emailsSortedByTest.Sort(StringComparer.Ordinal);
            CollectionAssert.AreEqual(emailsSortedByTest, emailsSortedByFrontEnd);
            return this;
        }
        public MentorsPage VerifyCorrectSorftingByEmailDesc()
        {
            List<string> emailsSortedByFrontEnd = GetEmails();
            List<string> emailsSortedByTest = new List<string>(emailsSortedByFrontEnd);
            emailsSortedByTest.Sort(StringComparer.Ordinal);
            emailsSortedByTest.Reverse();
            CollectionAssert.AreEqual(emailsSortedByTest, emailsSortedByFrontEnd);
            return this;
        }
    }
}
