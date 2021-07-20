using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace WHAT_PageObject
{
    public class StudentsPage : BasePageWithHeaderSidebar
    {
        const int STUDENTS_ON_PAGE = 10;
        const int STUDENTS_ON_PAGE_LESS = STUDENTS_ON_PAGE - 1;

        #region LOCATORS
        private By tbody=By.XPath("//tbody/tr");
        private By searchingField=By.XPath("//input[@type='text']");
        private By disabledStudentCheckBox = By.XPath("//input[@type ='checkbox']");
        private By addStudentButton = By.CssSelector("div:nth-child(4) > button");
        private By previousPage = By.CssSelector("nav > ul:nth-child(1) > li > button");
        private By nextPage = By.CssSelector("nav > ul:nth-child(3) > li > button");
        private By studentsCount = By.CssSelector(".col-2:nth-child(2)");
        private By countPages = By.CssSelector("li[class='page-item']");
        private By alert = By.XPath("//button[text()='Student information has been edited successfully']");
        #endregion

        public StudentsPage(IWebDriver driver) : base(driver)
        {

        }

        public int GetRangeStudent() => STUDENTS_ON_PAGE;
        private By GetPathToStudentInfo(int studentNumber, RowOfElement row) => By.XPath($@"//table/tbody/tr[{studentNumber}]/td[{(int)row}]");
        public int GetCountOfPages()=>driver.FindElements(countPages).Count-2;
        private By StudentsName(int rowNumber) => By.XPath($"//tr[{rowNumber}]/td[2]");

        public StudentsPage ClickPreviousPage()
        {
            driver.FindElement(previousPage).Click();
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
                driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.Id));
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
            return driver.FindElement(alert).GetAttribute("value");
        }

        public Dictionary<int, string[]> GetStudentsFromTable()
        {
            WaitStudentsLoad();
            Dictionary<int, string[]> studentsTable = new Dictionary<int, string[]>();
            int interval = STUDENTS_ON_PAGE;
            int studentNumber = 1;
            while (IsStudentDisplayed(studentNumber) || studentNumber == interval + 1)

            {
                try
                {
                    if (studentNumber >= STUDENTS_ON_PAGE - STUDENTS_ON_PAGE_LESS && studentNumber <= STUDENTS_ON_PAGE)
                    {
                        int studentId = int.Parse(driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.Id)).Text);
                        string studentFirstName = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.FirstName)).Text;
                        string studentLastName = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.LastName)).Text;
                        string studentEmail = driver.FindElement(GetPathToStudentInfo(studentNumber, RowOfElement.Email)).Text;
                        studentsTable.Add(studentId, new string[] { studentFirstName, studentLastName, studentEmail });
                        studentNumber++;
                    }
                    else
                    {
                        driver.FindElement(nextPage).Click();
                        interval += STUDENTS_ON_PAGE;
                        studentNumber = studentNumber- STUDENTS_ON_PAGE;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            for (int i = 1; i <= GetCountOfPages()-1; i++)
            {
                driver.FindElement(previousPage).Click();
            }
            return studentsTable;
        }

        public int GetCountStudents()

        {
            WaitStudentsLoad();
            string[] textFromStudentsCount = driver.FindElement(studentsCount).Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(textFromStudentsCount[0]);
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
