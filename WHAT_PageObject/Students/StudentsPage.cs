using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;

namespace WHAT_PageObject
{
    public class StudentsPage : BasePageWithHeaderSidebar
    {
        private By _searchingFieldLocator=By.XPath("//input[@class='search__searchInput___34nMl']");
        private By  _controlBarDisabledStudentsLocator = By.XPath("//input[@id='show - disabled - check']");
        private By _addStudentButtonLocator = By.XPath("//*[@id='root']/div/div/div[2]/div/div/div[4]/button");
        private By _sortingListLocator = By.CssSelector("thead/tr");
        private By _studentsListLocator = By.XPath("//*[@id='root']/div/div/div[2]/div/table/tbody/");

        public Dictionary<string, bool> arrowsState=new Dictionary<string, bool>();

        public StudentsPage(IWebDriver driver) : base(driver)
        {

        }


        public Dictionary<string, bool> GetArrowsState()
        {
            IList<IWebElement> _sortingList = driver.FindElements(_sortingListLocator);
            int indexOfList = 1;
            foreach (var sortingElement in _sortingList)
            {
                IWebElement sortingElementStatus= sortingElement.FindElement(By.TagName($"th:nth-child({indexOfList}) > span"));
                if (sortingElementStatus.GetCssValue("data-sorted-by-ascending") == Convert.ToString(1))
                {
                    arrowsState.Add(sortingElementStatus.Text, true);
                }
                else
                {
                    arrowsState.Add(sortingElementStatus.Text, false);
                }
                indexOfList++;
            }
            return arrowsState;
        }
        public StudentsEditPage ClickIconEditStudent(uint studentNumber)
        {
            
            IWebElement currentStudent;
            if (studentNumber >= 1 && studentNumber <= 10)
            {
                currentStudent = driver.FindElement(By.XPath($"//*[@id='root']/div/div/div[2]/div/table/tbody/tr[{studentNumber}]/td[5]/svg"));
                currentStudent.Click();
            }
            return new StudentsEditPage(driver);
        }

        public StudentsEditPage ClickChoosedStudent(uint studentNumber)
        {
            int studentsCount = int.Parse(driver.FindElement(By.XPath("//div[@class='col-2 text-right']")).Text);
            IWebElement currentStudent;
            uint intreval = 10;
            bool isStudentFound = false;
            do
            {
                if (studentNumber >= intreval - 9 && studentNumber <= intreval)
                {
                    currentStudent = driver.FindElement(By.XPath($"//*[@id='root']/div/div/div[2]/div/table/tbody/tr[{studentNumber}]"));
                    currentStudent.Click();
                    isStudentFound = true;
                }
                else
                {
                    intreval += 10;
                    driver.FindElement(By.XPath("//button[@class='page-link pagination__link___2AEaH']")).Click();
                }
            } while (!isStudentFound || intreval<= (studentsCount/10)+1);
        
            return new StudentsEditPage(driver);

        }
   
        private  bool IsShowsDisabledStudents()
        {
            IWebElement _controlBarDisabledStudents = driver.FindElement(_controlBarDisabledStudentsLocator);
            if (_controlBarDisabledStudents.GetCssValue("value") == "true")
            {
                return true;
            }
            return false;
        }

        public UnassignedUsers ClickAddStudentButton()
        {
            IWebElement _addStudentButton = driver.FindElement(_addStudentButtonLocator);
             _addStudentButton.Click();
             return new UnassignedUsers(driver);
        }
        
        public StudentsPage FillSearchingField(string inputingSentence)
        {
            IWebElement _searchingField = driver.FindElement(_searchingFieldLocator);
            _searchingField.Click();
            _searchingField.SendKeys(inputingSentence + Keys.Enter);
            _searchingField.Clear();
            return this;
        }

    }
}
