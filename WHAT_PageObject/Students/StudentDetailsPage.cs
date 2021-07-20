using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class StudentDetailsPage : BasePageWithHeaderSidebar
    {
        #region LOCATORS
        private By editDetailsNavLink = By.LinkText("Edit student details");
        private By studentDetails(RowOfDetails row) => By.XPath($"//div[{(int)row}]/div[2]/span[not(contains(.,'▼'))]");
        #endregion

        public StudentDetailsPage(IWebDriver driver) : base(driver)
        {

        }

        public EditStudentDetailsPage ClickEditStudentsDetaisNav()
        {
            driver.FindElement(editDetailsNavLink).Click();
            return new EditStudentDetailsPage(driver);
        }

        public string[] GetTexFromAllField()
        {
            return new string[]{
            driver.FindElement(studentDetails(RowOfDetails.FirstName)).Text,
            driver.FindElement(studentDetails(RowOfDetails.LastName)).Text,
            driver.FindElement(studentDetails(RowOfDetails.Email)).Text
            };
        }
    }
}
