using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_PageObject
{
    public class MyProfilePage : BasePageWithHeaderSidebar
    {
        public MyProfilePage(IWebDriver driver) : base(driver)
        {
        }
    }
}
