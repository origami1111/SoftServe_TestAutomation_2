using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace WHAT_PageFactory
{
    public class CoursesPage : Sidebar
    {
        [FindsBy(How = How.XPath, Using = "//tr[@data-student-id='1']/td[2]")]
        [CacheLookup]
        private IWebElement courseElement;

        public CoursesPage(IWebDriver driver) : base(driver)
        {
        }

        public string ReadCourseName()
        {
            return courseElement.Text;
        }

        public CourseDetailsPage ClickCourseName()
        {
            courseElement.Click();
            return new CourseDetailsPage(driver);
        }
    }
}
