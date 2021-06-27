using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;

namespace WHAT_PageObject
{
    public class UnassignedUsers: BasePageWithHeaderSidebar
    {
        public UnassignedUsers(IWebDriver driver) : base(driver)
        {

        }
    }
}
