using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class StudentsPage : BasePageWithHeaderSidebar
    {
        const int STUDENTS_ON_PAGE = 9;
        const int STUDENTS_ON_PAGE_LESS = STUDENTS_ON_PAGE - 1;

        #region LOCATORS
        private By tbody=By.XPath("//tbody/tr");
        private By searchingField=By.XPath("//input[@type='text']");
        private By disabledStudentCheckBox = By.XPath("//input[@type ='checkbox']");
        private By addStudentButton = By.CssSelector("div:nth-child(4) > button");
        private By previousPage = By.CssSelector("nav > ul:nth-child(1) > li > button");
        private By nextPage = By.CssSelector("nav > ul:nth-child(3) > li > button");
        private By studentsCount = By.CssSelector(".col-2:nth-child(2)");
        private By alert = By.CssSelector(".fade");
        #endregion
        
        private By StudentsName(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[2]");

        private By SortingRow(RowOfElement row) => By.XPath($"//tr/th[{(int)row}]/span");

        private By GetPathToStudentInfo(int studentNumber, RowOfElement row) => By.XPath($@"//table/tbody/tr[{studentNumber}]/td[{(int)row}]");
        
        public int GetCountOfPages()
        {
            int a = GetCountStudents();
            if (GetCountStudents() % STUDENTS_ON_PAGE != 0)
            {
                return (GetCountStudents() / STUDENTS_ON_PAGE)+1;
            }
            return GetCountStudents() / STUDENTS_ON_PAGE;
        }

        public StudentsPage(IWebDriver driver) : base(driver)
        {

        }

        public StudentsPage ClickPreviousPage()
        {
            driver.FindElement(previousPage).Click();
            return this;
        }

        public StudentsPage ClickSortingRow(RowOfElement row)
        {
            driver.FindElement(SortingRow(row)).Click();
            return this;
        }

        public StudentsPage ClickNextPage()
        {
            driver.FindElement(nextPage).Click();
            return this;
        }

        public StudentsPage ClickDisabledStudents_CheckBox()
        {
            driver.FindElement(disabledStudentCheckBox).Click();
            return this;
        }

        public void WaitStudentsLoad()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            IWebElement firstResult = wait.Until(e => e.FindElement(tbody));
        }

        private bool IsStudentDisplayed(int studentNumber)
        {
            try
            {
                driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.FirstName));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetAlertText()
        {
            return driver.FindElement(alert).Text;
        }

        private void AddStudentToList(ref List<string[]> studentsTable, ref int studentNumber)
        {
            string studentFirstName = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.FirstName)).Text;
            string studentLastName = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.LastName)).Text;
            string studentEmail = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.Email)).Text;
            studentsTable.Add(new string[] { studentFirstName, studentLastName, studentEmail });
            studentNumber++;
        }

        private void AddAllStudentFromPageToList (ref List<string[]> studentsTable, ref int studentNumber, ref int interval, ref int currPage)
        {
            for (int currStudent = 1; currStudent <= STUDENTS_ON_PAGE; currStudent++)
            {
                AddStudentToList(ref studentsTable, ref studentNumber);
            }
            driver.FindElement(nextPage).Click();
            interval += STUDENTS_ON_PAGE;
            studentNumber = studentNumber - STUDENTS_ON_PAGE;
            currPage++;
        }

        public List<string[]> GetStudentsFromTable()
        {
            WaitStudentsLoad();
            List<string[]> studentsTable = new List<string[]>();
            int interval = STUDENTS_ON_PAGE;
            int countPage = GetCountOfPages();
            int studentNumber = 1;
            for(int currPage = 1; currPage < countPage;)
            {
               AddAllStudentFromPageToList(ref studentsTable, ref studentNumber, ref interval, ref currPage);
            }
            while (IsStudentDisplayed(studentNumber))
            {
                try
                {
                    AddStudentToList(ref studentsTable, ref studentNumber);
                }
                catch (Exception)
                {
                    break;
                }
            }
            for (int currPage = 1; currPage < countPage; currPage++)
            {
                driver.FindElement(previousPage).Click();
            }
            return studentsTable;
        }

        public int GetCountStudents()
        {
            WaitStudentsLoad();
            string[] textFromStudentsCount = driver.FindElement(studentsCount).Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(textFromStudentsCount[2]);
        }
             
        public StudentDetailsPage ClickChoosedStudent(int studentNumber)

        {
            while (true)
            {
                try
                {
                    if (studentNumber >= STUDENTS_ON_PAGE - STUDENTS_ON_PAGE_LESS && studentNumber <= STUDENTS_ON_PAGE)
                    {
                        driver.FindElement(StudentsName(studentNumber)).Click();
                        return new StudentDetailsPage(driver);
                    }
                    else
                    {
                        driver.FindElement(nextPage).Click();
                        studentNumber -= STUDENTS_ON_PAGE;
                    }
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        public UnassignedUsersPage ClickAddStudentButton()
        {
            IWebElement addStudentBtnEl = driver.FindElement(addStudentButton);
            addStudentBtnEl.Click();
            return new UnassignedUsersPage(driver);
        }

        public StudentsPage FillSearchingField(string inputingSentence)
        {
            WaitStudentsLoad();
            IWebElement searchingFieldEl = driver.FindElement(searchingField);
            searchingFieldEl.Click();
            searchingFieldEl.Clear();
            searchingFieldEl.SendKeys(inputingSentence + Keys.Enter);
            return this;
        }
    }
}
