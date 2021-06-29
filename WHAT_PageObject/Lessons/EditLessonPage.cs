using OpenQA.Selenium;

namespace WHAT_PageObject
{
    public class EditLessonPage : BasePageWithHeaderSidebar
    {
        public EditLessonPage(IWebDriver driver) : base(driver)
        {

        }

        private By lessonThema = By.Id("inputLessonTheme");
        private By dateTimePicker = By.Id("choose-date/time");
        private By saveButton = By.XPath("//button[@type='submit']");
        private By cancelButton = By.XPath("//button[contains(.,'Cancel')]");

        public EditLessonPage FillLessonTheme(string tema)
        {
            FillField(lessonThema, tema);
            return this;
        }

        public EditLessonPage FillDateTime(string dataTime)
        {
            FillField(dateTimePicker, dataTime);
            return this;
        }

        public LessonsPage ClickSaveButton()
        {
            ClickItem(saveButton);
            return new LessonsPage(driver);
        }

        public LessonsPage ClickCancelButton()
        {
            ClickItem(cancelButton);
            return new LessonsPage(driver);
        }
    }
}
