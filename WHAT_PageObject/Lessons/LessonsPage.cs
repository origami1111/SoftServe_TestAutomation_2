using OpenQA.Selenium;
using System;
using System.Threading;

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
        
        public enum Column
        {
            Id = 1,
            ThemeName = 2,
            Date = 3,
            Time = 4,
            Edit = 5,
        }
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
            Thread.Sleep(20000);
            return this;
        }


    }
}
