using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;

namespace WHAT_PageObject
{
    public class StudentsEditPage: BasePageWithHeaderSidebar
    {
       
        private By _detailsNavLink=By.LinkText("Student details");
        private By _editNavLink = By.LinkText("Edit student details");
        private By _clearButton = By.XPath("//button[@class='w-100 btn btn-secondary edit-students-details__button___WOMG6']");
        private By _saveButton = By.XPath("//button[@class='w-100 btn btn-info edit-students-details__button___WOMG6']");
        private By _firstName = By.Id("firstName");
        private By _lastName = By.Id("lastName");
        private By _email = By.Id("email");

        private Queue<string> _allField= new Queue<string>();

        public StudentsEditPage clickClearButton()
        {
            driver.FindElement(_clearButton).Click();
            return this;
        }

        private StudentsEditPage GetTexFromAllField()
        {
            _allField.Enqueue(driver.FindElement(_firstName).GetCssValue("value"));
            _allField.Enqueue(driver.FindElement(_lastName).GetCssValue("value"));
            _allField.Enqueue(driver.FindElement(_email).GetCssValue("value"));
            return this;
        }
        public StudentsEditPage(IWebDriver driver) : base(driver)
        {
            
        }





    }
}
