using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class CoursesPage : BasePageWithHeaderSidebar
    {
        public CoursesPage(IWebDriver driver) : base(driver)
        {
        }

        private By Course(string courseNumber) =>
            By.XPath($"//tr[@data-student-id={courseNumber}]/td[2]");

        public string ReadCourseName(string courseNumber)
        {
            var course = driver.FindElement(Course(courseNumber));

            return course.Text;
        }

        public CourseDetailsPage ClickCourseName(string courseNumber)
        {
            var course = driver.FindElement(Course(courseNumber));
            course.Click();

            return new CourseDetailsPage(driver);
        }
    }
}
