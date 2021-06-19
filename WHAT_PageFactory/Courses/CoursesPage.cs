using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace WHAT_PageFactory
{
    public class CoursesPage : BasePageWithSidebar
    {
        [FindsBy(How = How.XPath, Using = "//tr[@data-student-id='1']/td[2]")]
        [CacheLookup]
        private IWebElement course;

        public CoursesPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public string ReadCourseName()
        {
            return course.Text;
        }

        public CourseDetailsPage ClickCourseName()
        {
            course.Click();

            return new CourseDetailsPage(driver);
        }
    }
}
