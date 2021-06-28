using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using System.Collections;
using WHAT_PageObject;

namespace WHAT_Tests
{
    public class WebDriverArgs : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { (IWebDriver)new FirefoxDriver() };
            yield return new object[] { (IWebDriver)new ChromeDriver() };
        }
    }

    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [TestFixtureSource(typeof(WebDriverArgs))]
    public class DriversTests : IDisposable
    {
        private IWebDriver driver;
        private CoursesPage coursesPage;

        public DriversTests(IWebDriver driver)
        {
            this.driver = driver;
        }

        [SetUp]
        public void SetUp()
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("http://localhost:8080/");
            
            coursesPage = new SignIn(driver)
                            .SignInAsAdmin()
                            .SidebarNavigateTo<CoursesPage>();
        }

        [TearDown]
        public void TearDown()
        {
            coursesPage.Logout();
            driver.Quit();
        }

        [TestCase]
        public void AddCourse_EmptyName()
        {
            var expected = "This field is required";
            var anyData = "Test";

            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(anyData)
                                    .DeleteTextWithBackspaces(anyData.Length);

            Assert.True(expected == actual.GetErrorMessage() && actual.IsSaveButtonDisabled());
        }

        void IDisposable.Dispose()
        {
            driver?.Dispose();
            driver = null;
        }
    }
}