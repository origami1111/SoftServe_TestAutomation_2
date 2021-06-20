using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace WHAT_PageFactory
{
    public class CourseDetailsPage : Sidebar
    {
        [FindsBy(How = How.XPath, Using = "//h3[.='Course Details']")]
        [CacheLookup]
        private IWebElement courseDetailsElements;

        [FindsBy(How = How.XPath, Using = "//div[@class='container']//div[@class='row']/div[2]")]
        [CacheLookup]
        private IWebElement courseNameDetails;

        public CourseDetailsPage(IWebDriver driver) : base(driver)
        {
        }

        public string ReadCourseNameDetails()
        {
            return courseNameDetails.Text;
        }
    }
}
