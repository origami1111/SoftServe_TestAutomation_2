﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;


namespace WHAT_PageObject
{
    public class SecretariesPage : BasePageWithHeaderSidebar
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

        // Get user with displayed data (by index).
        // Row click.
        //* 'Add a secretary' button click.
        //- Navigate buttons clicks.
        //* SortBy
        // GetTotalUsersShowedAmount.
        // GetOnPageUsersShowedAmount.
        // *EditSecretary
        //* GetShowedUsersAmount.       
        // 
        //* Get list users on page sorted by different params.

        

        // Locators
        By addSecretaryButton = By.XPath("//span[contains(.,'Add a secretary')]");
        By countUsersReport = By.XPath("//span[contains(.,'secretaries')]");
        By searchField = By.XPath("//input[@type='text']");
        By disabledSwitch = By.XPath("//label[contains(.,'Disabled Secretaries')]");
        By visibleUsersSelect = By.Id("change-visible-people");
        By prevPageLink = By.XPath("//button[contains(.,'<')]");
        By nextPageLink = By.XPath("//button[contains(.,'>')]");
        //By indexColumn = By.XPath("//span[@data-sorting-param='index']"); 
        //By firstNameColumn = By.XPath("//span[@data-sorting-param='firstName']"); 
        //By lastNameColumn = By.XPath("//span[@data-sorting-param='lastName']"); 
        //By emailColumn = By.XPath("//span[@data-sorting-param='email']"); 
        By sortedBy = By.XPath("//thead//th[1]/span");
        By userData = By.XPath("//tbody/tr/td[1]");

       
        public SecretariesPage(IWebDriver driver) : base (driver)
        {
           
        }

        public UnassignedUsersPage AddSecretary()
        {
            driver.FindElement(addSecretaryButton).Click();
            return new UnassignedUsersPage(driver);
        }

        public EditSecretaryPage EditSecretary (int index)
        {
            driver.FindElement(By.XPath($"//td[@data-secretary-id={index}]")).Click();
            return new EditSecretaryPage(driver);
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

        public int GetUsersOnPage()
        {
            SelectElement selectedOption = new SelectElement(driver.FindElement(visibleUsersSelect));
            return Int32.Parse(selectedOption.SelectedOption.Text);
            
        }

        public void SelectUsersOnPage(showedUsers showedUsers)
        {
            SelectElement selectedOption = new SelectElement(driver.FindElement(visibleUsersSelect));
            selectedOption.SelectByIndex((int)showedUsers);
        }
        private string GetUserData (int columnNumber, int rowNumber)
        {
            return driver.FindElement(By.XPath($"//tbody/tr[{rowNumber}]/td[{columnNumber}]")).Text;            
        }

        public int GetShowedUsersAmount ()
        {
            return driver.FindElements(userData).Count;
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
