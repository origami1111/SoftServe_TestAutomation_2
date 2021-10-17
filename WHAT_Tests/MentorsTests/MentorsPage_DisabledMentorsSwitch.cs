using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class MentorsPage_DisabledMentorsSwitch : TestBase
    {
        private MentorsPage mentorsPage;

        [SetUp]
        public void Precondition()
        {
            log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            mentorsPage.Logout();
        }

        [Test, Description("DP213-158")]
        public void TestDisabledMentorsSwitch()
        {
            var adminCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
            var secretaryCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Secretary);
            mentorsPage = new SignInPage(driver)
                .SignInAsAdmin(adminCredentials.Email, adminCredentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .ClickDisabledMentorsToggle()
                .WaitUntilMentorsTableLoads()
                .Logout()
                .SignInAsSecretar(secretaryCredentials.Email, secretaryCredentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .ClickDisabledMentorsToggle()
                .WaitUntilMentorsTableLoads();
        }
    }
}
