using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class SecretariesSortingTests : TestBase
    {
        private SecretariesPage secretariesPage;

        [SetUp]
        public void Setup()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            secretariesPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>();
        }

        [TearDown]
        public void Down()
        {
            secretariesPage.Logout();
        }

        [Test]
        public void ByNameSortingVerify()
        {
            List<string> expected = secretariesPage.GetDataList(ColumnName.firstName);
            expected.Sort();

        }
        
    }
}