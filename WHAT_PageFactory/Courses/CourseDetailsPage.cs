using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace WHAT_PageFactory

{
    public class CourseDetailsPage : BasePageWithSidebar
    {
        [FindsBy(How = How.XPath, Using = "//h3[.='Course Details']")]
        [CacheLookup]
        private IWebElement courseDetails;

        [FindsBy(How = How.XPath, Using = "//div[@class='container']//div[@class='row']/div[2]")]
        [CacheLookup]
        private IWebElement courseNameDetails;

        public CourseDetailsPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public string ReadCourseNameDetails()
        {
            return courseNameDetails.Text;
        }
    }
}
