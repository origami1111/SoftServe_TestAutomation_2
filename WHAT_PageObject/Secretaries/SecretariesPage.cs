using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WHAT_PageObject
{
    public class SecretariesPage : BasePageWithHeaderSidebar
    {

        
        By addSecretaryButton = By.XPath("//span[contains(.,'Add a secretary')]");
        By disabledSwitch = By.XPath("//label[contains(.,'Disabled Secretaries')]");
        By searchField = By.XPath("//input[@type='text']");
        By visibleUsersSelect = By.Id("change-visible-people");
        private int usersOnPageIndex = 0;
        private int usersTotalIndex = 2;
        By countUsersReport = By.XPath("//span[contains(.,'secretaries')]");       
        By prevPageLink = By.XPath("//button[contains(.,'<')]");
        By nextPageLink = By.XPath("//button[contains(.,'>')]");
        //By indexColumn = By.XPath("//span[@data-sorting-param='index']"); 
        //By firstNameColumn = By.XPath("//span[@data-sorting-param='firstName']"); 
        //By lastNameColumn = By.XPath("//span[@data-sorting-param='lastName']"); 
        //By emailColumn = By.XPath("//span[@data-sorting-param='email']"); 
        By sortedBy = By.XPath("//thead//th[1]/span");
        By userData = By.XPath("//tbody/tr");
        By pages = By.XPath("//div[1]/nav/ul[2]/li");
       
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

        public SecretariesPage LastPage()
        {
            //driver.FindElements(pages).Last().Click();
            driver.FindElement(By.XPath("//div[1]/nav/ul[2]/li[2]")).Click();
            return this;
        }

        public SecretariesPage DisabledSwitch()
        {
            driver.FindElement(disabledSwitch).Click();
            return this;
        }

        public SecretariesPage SortBy (ColumnName column)
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

        public void SelectUsersOnPage(ShowedUsers showedUsers)
        {
            driver.FindElement(visibleUsersSelect).Click();
            SelectElement selectedOption = new SelectElement(driver.FindElement(visibleUsersSelect));
            selectedOption.SelectByIndex((int)showedUsers-1);
        }

        private string GetUserData (int columnNumber, int rowNumber)
        {
            return driver.FindElement(By.XPath($"//tbody/tr[{rowNumber}]/td[{columnNumber}]")).Text;            
        }

        public int GetShowedUsersAmount ()
        {
            return driver.FindElements(userData).Count;
        }

        public List<string> GetSortedList (ColumnName column)
        {
            int count = GetShowedUsersAmount();
            List<string> sortedList = new List <string> (count);
            for (int i = 1; i <= count; i++)
            {
                sortedList.Add(GetUserData((int)column, i));
            }
            return sortedList;
        }

        public int GetPagesAmount ()
        {
            IReadOnlyCollection <IWebElement> buttons = driver.FindElements(pages);
            return Int32.Parse(buttons.Last().Text);
        }

        public int GetReportedUsersOnPage()
        {
            return Int32.Parse(driver.FindElement(countUsersReport).Text.Split(" ")[usersOnPageIndex]);
        }

        public int GetReportedUsersTotal ()
        {
            return Int32.Parse(driver.FindElement(countUsersReport).Text.Split(" ")[usersTotalIndex]);
        }
    }
    

}
