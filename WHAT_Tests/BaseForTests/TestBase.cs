using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [SetUpFixture]
    public abstract class TestBase
    {
        protected IWebDriver driver;
        public readonly string LinksPath = @"DataFiles/Links.json";
        public APIClient api = new APIClient();
        protected internal Logger log = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(ReaderUrlsJSON.ByName("SigninPage", LinksPath));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            var context = TestContext.CurrentContext;
            var testName = context.Test.FullName;
            if (context.Result.Outcome.Status == TestStatus.Passed)
            {
                log.Info($"{testName} {context.Result.Outcome.Status}");
                return;
            }
            foreach (var assertion in context.Result.Assertions)
            {
                log.Error($"{testName} {assertion.Status}:{Environment.NewLine}{assertion.Message}");
            }
        }
    }
}

