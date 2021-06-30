using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace WHAT_PageObject
{
    public class StudentsPage : BasePageWithHeaderSidebar
    {
        const int STUDENTS_ON_PAGE = 10;
        const int STUDENTS_ON_PAGE_LESS = STUDENTS_ON_PAGE - 1;

        #region LOCATORS

        private By _searchingField=By.XPath("//input[@type='text']");
        private By  _controlBarDisabledStudents = By.XPath("//input[@id='show - disabled - check']");
        private By _addStudentButton = By.XPath("//*[@id='root']/div/div/div[2]/div/div/div[4]/button");
        private By _sortingList = By.CssSelector("thead/tr");
        private By _previousPage = By.XPath("//*[@id='root']/div/div/div[1]/div[2]/nav/ul[1]/li/button");
        private By _nextPage = By.XPath("//*[@id='root']/div/div/div[1]/div[2]/nav/ul[3]/li/button");
        private By _studentsCount = By.CssSelector(".col-2:nth-child(2)");
        private By _countPages = By.CssSelector("ul:nth-child(2) > li:nth-child(4) > button");
        #endregion
        private By GetPathToStudentInfo(int studentNumber, RowOfEl row) => By.XPath($@"//table/tbody/tr[{studentNumber}]/td[{(int)row}]");

        private By StudentsName(int rowNumber) =>
            By.XPath($"//tr[{rowNumber}]/td[2]");
        public StudentsPage(IWebDriver driver) : base(driver)
        {

        }
        public uint GetCountOfPages()
        {
            if (driver.FindElement(_countPages).Enabled)
            {
                Thread.Sleep(2000);
                return Convert.ToUInt32(driver.FindElement(_countPages).Text);
            }
            else
            {
                return 0;
            }
        }
        public StudentsPage ClickPreviousPage()
        {
            driver.FindElement(_previousPage).Click();
            return this;
        }

        public StudentsPage ClickNextPage()
        {
            driver.FindElement(_nextPage).Click();
            return this;
        }

        public StudentsPage ClickControlBarDisabledStudentsPage()
        {
            driver.FindElement(_controlBarDisabledStudents).Click();
            return this;
        }

       
        public void WaitStudentsLoad()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            IWebElement firstResult = wait.Until(e => e.FindElement(_searchingField));
        }
        private bool IsStudentDisplayed(int studentNumber)
        {
            try
            {
                driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfEl.Id));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Dictionary<int, string[]> GetStudentsFromTable()
        {
            Dictionary<int, string[]> studentsTable = new Dictionary<int, string[]>();
            int interval = STUDENTS_ON_PAGE;
            int studentNumber = 1; 
            while (IsStudentDisplayed(studentNumber))
            {
                if (studentNumber >= interval - STUDENTS_ON_PAGE_LESS && studentNumber <= interval)
                {
                    int studentId = int.Parse(driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfEl.Id)).Text);
                    string studentFirstName = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfEl.FirstName)).Text;
                    string studentLastName = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfEl.LastName)).Text;
                    studentsTable.Add(studentId, new string[] { studentFirstName, studentLastName });
                }
                else
                {
                    driver.FindElement(_nextPage).Click();
                    interval += interval;
                    studentNumber--;
                }
                studentNumber++;
            }
            return studentsTable;
        }

        public uint  GetCountStudents()
        {
            Thread.Sleep(2000);
            string[] textFromStudentsCount = driver.FindElement(_studentsCount).Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToUInt32(textFromStudentsCount[0]);
        }

             
        public StudentsEditPage ClickChoosedStudent(int studentNumber)
        {
            var course = driver.FindElement(StudentsName(studentNumber));
            course.Click();

            return new StudentsEditPage(driver);

        }
        public StudentsEditPage ClickPencilLink(int rowNumber)
        {
            IWebElement table = driver.FindElement(By.Name("table"));
            var rows = table.FindElements(By.Name("tr"));
            var rowTds = rows[rowNumber - 1].FindElements(By.TagName("td"));

            foreach (var td in rowTds)
            {
                var pencil = td.FindElement(By.Id("Edit"));
                pencil.Click();
                break;
            }

            return new StudentsEditPage(driver);
        }

        public UnassignedUsersPage ClickAddStudentButton()
        {
            IWebElement addStudentBtnEl = driver.FindElement(_addStudentButton);
            addStudentBtnEl.Click();
            return new UnassignedUsersPage(driver);
        }

        public StudentsPage FillSearchingField(string inputingSentence)
       {
            IWebElement searchingFieldEl = driver.FindElement(_searchingField);
            searchingFieldEl.Click();
            searchingFieldEl.Clear();
            searchingFieldEl.SendKeys(inputingSentence + Keys.Enter);
            return this;
        }

    }
}
