using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class UnassignedUsersPage : BasePageWithHeaderSidebar
    {
        private By
                userNotFound = By.XPath("//tbody//tr/td[contains (text(), 'not found')]"),
                inputField = By.XPath("//input[@class = 'search__searchInput___34nMl']");
        private SelectElement select;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private Dictionary<Type, By> listNames = new Dictionary<Type, By>()
        {
            [typeof(MentorsPage)] = By.XPath($"//tr[@class='list-of-mentors__table-rows___VLZnb']"),
            [typeof(SecretariesPage)] = By.XPath($"//tr[@class='list-of-secretaries__table-row___3cdmz']"),
            [typeof(StudentsPage)] = By.XPath($"//tr[@class='list-of-students__table-row___2X3jB']")
        };

        public enum ColumnName
        {
            Id = 1,
            Name = 2,
            Surname = 3,
            Email = 4,
            SelectRole = 5,
            AddRole = 6
        }

        private By TableCell(int rowNumber, ColumnName column)
        {
            if ((int)column == 5)
            {
                return By.XPath($"//tr[{rowNumber}]/td[{(int)column}]/div/select[@class='unassigned-list__select___UNLgl']");
            }
            else if ((int)column == 6)
            {
                return By.XPath($"//tr[{rowNumber}]/td[5]/div/button[text()='Add role']");
            }

            return By.XPath($"//tr[{rowNumber}]/td[{(int)column}]");
        }

        public UnassignedUsersPage(IWebDriver driver) : base(driver)
        {

        }

        public UnassignedUsersPage AddStudentRole(int rowNumber)
        {
            FirstName = driver.FindElement(TableCell(rowNumber, ColumnName.Name)).Text;
            LastName = driver.FindElement(TableCell(rowNumber, ColumnName.Surname)).Text;
            Email = driver.FindElement(TableCell(rowNumber, ColumnName.Email)).Text;

            select = new SelectElement(driver.FindElement(TableCell(rowNumber, ColumnName.SelectRole)));
            select.SelectByText("student");

            ClickItem(TableCell(rowNumber, ColumnName.AddRole));

            return this;
        }

        public UnassignedUsersPage AddMentorRole(int rowNumber)
        {
            FirstName = driver.FindElement(TableCell(rowNumber, ColumnName.Name)).Text;
            LastName = driver.FindElement(TableCell(rowNumber, ColumnName.Surname)).Text;
            Email = driver.FindElement(TableCell(rowNumber, ColumnName.Email)).Text;

            select = new SelectElement(driver.FindElement(TableCell(rowNumber, ColumnName.SelectRole)));
            select.SelectByText("mentor");

            ClickItem(TableCell(rowNumber, ColumnName.AddRole));

            return this;
        }

        public UnassignedUsersPage AddSecretaryRole(int rowNumber)
        {
            FirstName = driver.FindElement(TableCell(rowNumber, ColumnName.Name)).Text;
            LastName = driver.FindElement(TableCell(rowNumber, ColumnName.Surname)).Text;
            Email = driver.FindElement(TableCell(rowNumber, ColumnName.Email)).Text;

            select = new SelectElement(driver.FindElement(TableCell(rowNumber, ColumnName.SelectRole)));
            select.SelectByText("secretary");

            ClickItem(TableCell(rowNumber, ColumnName.AddRole));

            return this;
        }

        public bool UserVerify<T>(string firstName, string lastName, string email)
        {
            WaitingElement(listNames[typeof(T)]);

            FillField(inputField, firstName);

            if (!IsElementPresent(userNotFound))
            {
                var list = driver.FindElements(listNames[typeof(T)]);
                foreach (var item in list)
                {
                    if (email == item.FindElement(By.XPath($"td[{(int)ColumnName.Email}]")).Text
                                 && firstName == item.FindElement(By.XPath($"td[{(int)ColumnName.Name}]")).Text
                                 && lastName == item.FindElement(By.XPath($"td[{(int)ColumnName.Surname}]")).Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private void WaitingElement(By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(e => e.FindElement(by));
        }
    }
}
