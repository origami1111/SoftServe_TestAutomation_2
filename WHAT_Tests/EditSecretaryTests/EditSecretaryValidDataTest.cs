using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class EditSecretaryValidDataTest: TestBase
    {
        private UnassignedUsersPage findUser;
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }


        #region TestData

        private static IEnumerable<TestCaseData> ValidCasesData()
        {

            yield return new TestCaseData("Hf", "Df", "sec23131@gmail.com");

            yield return new TestCaseData("TRwRDBBkdAySScJbZpKMTMBllpkWQHJuREbfDBsrlNMIeZXzRV",
                                          "TRSRDVBkdAySScJbZpKMTMBllpkWQHJuREbfDBsrlNMIeZXzRV",
                                          "va23131@gmail.com");

            yield return new TestCaseData("fHYJCJZhECrGcPxHvLcBcupPFebcWbNCjyyWiVMClRGbwXEqm",
                                          "FbkJCJZhECrGcPxHvLcBvzsPFebcWbNCjyyWiVMClRGbwXEqm",
                                          "vsva23131@gmail.com");

            yield return new TestCaseData("Har", "Dar", "scbx2331@gmail.com");

            yield return new TestCaseData("GSAijsaoijfskafmiafiwsdsadas",
                                          "GVnijsaoijfskafmiafiwsdsadas",
                                          "ssb331@gmail.com");
        }

        #endregion

        private EditSecretaryPage editSecretaryPage;

        [SetUp]
        public void Set()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            editSecretaryPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>()
                            .EditSecretary(5);

            FirstName = editSecretaryPage.GetFirstName();
            LastName = editSecretaryPage.GetLastName();
            Email = editSecretaryPage.GetEmail();
        }

        [TearDown]
        public void Down()
        {
            editSecretaryPage.SidebarNavigateTo<SecretariesPage>()
                             .EditSecretary(5)
                             .Fill_FirstName(FirstName)
                             .Fill_LastName(LastName)
                             .Fill_Email(Email)
                             .ClickSaveButton();

            editSecretaryPage.Logout();  
        }

        [Test]

        [TestCaseSource(nameof(ValidCasesData))]

        public void ValidDataTest(string firstName, string lastName, string email)
        {

            editSecretaryPage.Fill_FirstName(firstName)
                             .Fill_LastName(lastName)
                             .Fill_Email(email)
                             .ClickSaveButton();

            findUser = new UnassignedUsersPage(driver);
            bool actual = findUser.UserVerify<SecretariesPage>(firstName,lastName,email);

            Assert.IsTrue(actual);
        }

    }
}

