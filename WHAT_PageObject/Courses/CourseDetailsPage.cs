using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class CourseDetailsPage : BasePageWithHeaderSidebar
    {
        public CourseDetailsPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CourseNameDetails => driver.FindElement(
            By.XPath("//div[@class='container']//div[@class='row']/div[2]"));

        public string GetCourseNameDetails()
        {
            return CourseNameDetails.Text;
        }
    }
}
