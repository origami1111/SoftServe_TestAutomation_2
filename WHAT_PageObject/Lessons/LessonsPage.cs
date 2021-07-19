using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WHAT_PageObject
{
    public class LessonsPage : BasePageWithHeaderSidebar
    {
        public LessonsPage(IWebDriver driver) : base(driver)
        {

        }

        private By addLessonButton = By.XPath("//span[contains(.,'Add a lesson')]");
        private By searchField = By.CssSelector(".search__searchInput___34nMl");
        private By paginationNextPage = By.XPath("//button[contains(.,'>')]");
        private By paginationPreviousPage = By.XPath("//button[contains(.,'<')]");
        private By countLessons = By.CssSelector(".col-2:nth-child(2)");
        private By successMessage = By.CssSelector(".fade");
        private By LessonThemaName(string rowNumber) => By.XPath($"//tr[{rowNumber}]/td[2]");
        private By LessonId(string rowNumber) => By.XPath($"//tr[{rowNumber}]/td[1]");
        private By EditLessonNumber(string rowNumber) => By.XPath($"//tr[{rowNumber}]/td[5]");
        private By elementsInTable = By.XPath("//tr[@class = 'list-of-lessons__table-row___16_kJ']");
        public string VerifySuccesMessage()
        {
            return driver.FindElement(successMessage).Text;
        }

        public AddLessonsPage ClickAddLessonButton()
        {
            ClickItem(addLessonButton);
            return new AddLessonsPage(driver);
        }

        public LessonsPage SearchByThemaName(string name)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(e => e.FindElements(elementsInTable));
            FillField(searchField, name);
            return this;
        }

        public LessonsPage ClickNextPageOnPagination()
        {
            ClickItem(paginationNextPage);
            return this;
        }

        public LessonsPage ClickPreviousPageOnPagination()
        {
            ClickItem(paginationPreviousPage);
            return this;
        }

        public int GetCountLessons()
        {
            string[] allText = driver.FindElement(countLessons).Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int count = Convert.ToInt32(allText[0]);
            return count;
        }

        public LessonsPage RefreshPage()
        {
            driver.Navigate().Refresh();
            return this;
        }

        public string GetLessonThemaName(string number)
        {
            return driver.FindElement(LessonThemaName(number)).Text;
        }

        public string GetLessonById(string number)
        {
            return driver.FindElement(LessonId(number)).Text;
        }

        public EditLessonPage ClickEditLesson(string number)
        {
            ClickItem(EditLessonNumber(number));
            return new EditLessonPage(driver);
        }
    }
}
