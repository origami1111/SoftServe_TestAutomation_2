using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace WHAT_PageObject
{
    public class UnassignedUsersPage : BasePageWithHeaderSidebar
    {

        private SelectElement select;

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

            select = new SelectElement(driver.FindElement(TableCell(rowNumber, ColumnName.SelectRole)));
            select.SelectByText("student");

            ClickItem(TableCell(rowNumber, ColumnName.AddRole));

            return this;

        }

        public UnassignedUsersPage AddMentorRole(int rowNumber)
        {

            select = new SelectElement(driver.FindElement(TableCell(rowNumber, ColumnName.SelectRole)));
            select.SelectByText("mentor");

            ClickItem(TableCell(rowNumber, ColumnName.AddRole));

            return this;

        }

        public UnassignedUsersPage AddSecretaryRole(int rowNumber)
        {

            select = new SelectElement(driver.FindElement(TableCell(rowNumber, ColumnName.SelectRole)));
            select.SelectByText("secretary");

            ClickItem(TableCell(rowNumber, ColumnName.AddRole));

            return this;

        }

    }
}
