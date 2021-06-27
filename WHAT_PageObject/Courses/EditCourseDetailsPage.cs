using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class EditCourseDetailsPage : BasePageWithHeaderSidebar
    {
        public EditCourseDetailsPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CourseName => driver.FindElement(
            By.XPath("//div[@class='container']//div[@class='row']/div[2]"));

        public string ReadCourseName()
        {
            return CourseName.Text;
        }

        public EditCourseDetailsPage EditCourseName()
        {

            return this;
        }
    }
}
