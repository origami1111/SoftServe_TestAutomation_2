using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using WHAT_PageObject.Base;

namespace WHAT_PageObject.Secretaries
{
    class SecretariesPage : BasePage
    {
        //        Locators:

        //* 'Add a secretary' button.
        //* Count users showed on page field.
        //* Count users field.
        //* Pagination navigate buttons.
        //* Head captions.
        //* Rows data.
        //* Sorting buttons.
        //* Select users on page
        //* SearchField 
        //* DisabledSwitch
        
        //          Methods:

        // Get first user with displayed data.
        // First row click.
        //* 'Add a secretary' button click.
        //- Navigate buttons clicks.
        //* SortBy
        // GetTotalUsersShowedAmount.
        // GetOnPageUsersShowedAmount.
        
        //* GetShowedUsersAmount.       
        // GetUsersOnPage.
        //* Get list users on page sorted by different params.


        // Locators
        By addSecretaryButton = By.XPath("//span[contains(.,'Add a secretary')]");
        By countUsersReport = By.XPath("//span[contains(.,'secretaries')]");
        By searchField = By.XPath("//input[@type='text']");
        By disabledSwitch = By.XPath("//label[contains(.,'Disabled Secretaries')]");
        By visibleUsers = By.Id("change-visible-people");
        By prevPageLink = By.XPath("//button[contains(.,'<')]");
        By nextPageLink = By.XPath("//button[contains(.,'>')]");
        By indexColumn = By.XPath("//span[@data-sorting-param='index']"); // //thead//th[1]/span
        By firstNameColumn = By.XPath("//span[@data-sorting-param='firstName']"); // //thead//th[2]/span
        By lastNameColumn = By.XPath("//span[@data-sorting-param='lastName']"); // //thead//th[3]/span
        By emailColumn = By.XPath("//span[@data-sorting-param='email']"); // //thead//th[4]/span
        By sortedBy = By.XPath("//thead//th[1]/span");
        By userData = By.XPath("//tbody/tr/td[1]");
        // "//tbody/tr[1]/td[1]"; 
        
        // ReaderFileCSV.ReadFileListCredentials("secretary_active.csv").Count; // Читать из файла

        //private void GetVisibleUsers ()
        //{
        //    SelectElement selectedOption = new SelectElement(driver.FindElement(visibleUsers));
        //    int visibleUsersReport = Int32.Parse(selectedOption.SelectedOption.Text);
        //}

        public enum columnName 
        {
            index = 1,
            firstName = 2,
            lastName = 3,
            email = 4
        }

        public SecretariesPage(IWebDriver driver) : base (driver)
        {
           
        }

        public SecretariesPage AddSecretaryClick()
        {
            driver.FindElement(addSecretaryButton).Click();
            return this;
            // return UnassigmentUsersPage;
        }

        public SecretariesPage PrevPage ()
        {
            driver.FindElement(prevPageLink).Click();
            return this;
        }

        public SecretariesPage NextPage()
        {
            driver.FindElement(nextPageLink).Click();
            return this;
        }

        public SecretariesPage DisabledSwitch ()
        {
            driver.FindElement(disabledSwitch).Click();
            return this;
        }

        public SecretariesPage SortBy (columnName column)
        {
            sortedBy = By.XPath($"//thead//th[{(int)column}]/span");
            driver.FindElement(sortedBy).Click();
            return this;
        }

        private string GetUserData (int columnNumber, int rowNumber)
        {
            return driver.FindElement(By.XPath($"//tbody/tr[{rowNumber}]/td[{columnNumber}]")).Text;            
        }

        public int GetShowedUsersAmount ()
        {
            int count = driver.FindElements(userData).Count;
            return count;
        }
        public SecretariesPage GetSortedList (columnName column)
        {
            int count = GetShowedUsersAmount();
            List<string> sortedList = new List <string> (count);
            for (int i = 1; i <= count; i++)
            {
                sortedList.Add(GetUserData((int)column, i));
            }
            return this;
        }
    }

    

}
