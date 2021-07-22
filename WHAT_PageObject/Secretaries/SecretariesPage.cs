using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class SecretariesPage : BasePageWithHeaderSidebar
    {
        WebDriverWait wait;

        #region Locators
        By addSecretaryButton = By.XPath("//span[contains(.,'Add a secretary')]");
        By disabledSwitch = By.XPath("//label[contains(.,'Disabled Secretaries')]");
        By searchField = By.XPath("//input[@type='text']");
        By visibleUsersSelect = By.Id("change-visible-people");
        By countUsersReport = By.XPath("//span[contains(.,'secretaries')]");
        By prevPageLink = By.XPath("//button[contains(.,'<')]");
        By nextPageLink = By.XPath("//button[contains(.,'>')]");
        By sortedBy = By.XPath("//thead//th[1]/span");
        By userData = By.XPath("//tbody/tr");
        By firstPage = By.XPath("//nav/ul[2]/li[1]");
        By lastPage = By.XPath("//nav/ul[2]/li[last()]");
        By lastUserIndex = By.XPath($"//tbody/tr[last()]/td[{(int)ColumnName.index}]");
        private int usersAtPageIndex = 0;
        private int usersTotalIndex = 2;
        #endregion

        public SecretariesPage(IWebDriver driver) : base(driver)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public UnassignedUsersPage AddSecretary()
        {
            driver.FindElement(addSecretaryButton).Click();
            return new UnassignedUsersPage(driver);
        }

        public EditSecretaryPage EditSecretary(int index)
        {
            driver.FindElement(By.XPath($"//td[@data-secretary-id={index}]")).Click();
            return new EditSecretaryPage(driver);
        }

        #region navigation
        public SecretariesPage PrevPage()
        {
            driver.FindElement(prevPageLink).Click();
            return this;
        }

        public SecretariesPage NextPage()
        {
            driver.FindElement(nextPageLink).Click();
            return this;
        }

        public SecretariesPage LastPage()
        {
            wait.Until(e => e.FindElement(lastPage));
            driver.FindElement(lastPage).Click();
            return this;
        }

        public SecretariesPage FirstPage()
        {
            driver.FindElement(firstPage).Click();
            return this;
        }
        #endregion

        #region notUsed
        public SecretariesPage DisabledSwitch()
        {
            driver.FindElement(disabledSwitch).Click();
            return this;
        }

        public SecretariesPage SortBy(ColumnName column)
        {
            sortedBy = By.XPath($"//thead//th[{(int)column}]/span");
            driver.FindElement(sortedBy).Click();
            return this;
        }

        private string GetUserData(int columnNumber, int rowNumber)
        {
            return driver.FindElement(By.XPath($"//tbody/tr[{rowNumber}]/td[{columnNumber}]")).Text;
        }

        public int GetReportedUsersAtPage()
        {
            return Int32.Parse(driver.FindElement(countUsersReport).Text.Split(" ")[usersAtPageIndex]);
        }

        public int GetReportedUsersTotal()
        {
            return Int32.Parse(driver.FindElement(countUsersReport).Text.Split(" ")[usersTotalIndex]);
        }

        public List<string> GetDataList(ColumnName column)
        {
            int count = GetShowedUsersAmount();
            List<string> dataList = new List<string>(count);
            for (int i = 1; i <= count; i++)
            {
                dataList.Add(GetUserData((int)column, i));
            }
            return dataList;
        }
        #endregion

        public bool GetPagesAmount(out int pagesAmount)
        {
            return Int32.TryParse(driver.FindElement(lastPage).Text, out pagesAmount);
        }

        public int GetShowedUsersAmount()
        {
            return driver.FindElements(userData).Count;
        }

        public bool GetUsersAtPage(out int usersAtPage)
        {
            wait.Until(e => e.FindElement(visibleUsersSelect));
            SelectElement selectedOption = new SelectElement(driver.FindElement(visibleUsersSelect));
            return Int32.TryParse(selectedOption.SelectedOption.Text, out usersAtPage);
        }

        public void SelectUsersAtPage(ShowedUsers usersAtPage)
        {
            driver.FindElement(visibleUsersSelect).Click();
            SelectElement selectedOption = new SelectElement(driver.FindElement(visibleUsersSelect));
            selectedOption.SelectByIndex((int)usersAtPage - 1);
        }

        public bool GetLastUserIndex(out int index)
        {
            return Int32.TryParse(LastPage().driver.FindElement(lastUserIndex).Text, out index);
        }
    }
}
