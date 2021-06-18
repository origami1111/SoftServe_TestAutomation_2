using OpenQA.Selenium;

namespace WHAT_PageObject

{
    public class CourseDetailsPage : BasePageWithSidebar
    {
        public CourseDetailsPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CourseDetailsLabel => driver.FindElement(By.XPath("//h3[.='Course Details']"));

        private IWebElement CourseNameDetails => driver.FindElement(
            By.XPath("//div[@class='container']//div[@class='row']/div[2]"));

        public string ReadCourseNameDetails()
        {
            return CourseNameDetails.Text;
        }
    }
}
