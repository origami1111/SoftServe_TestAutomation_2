using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class EditSecretaryInvalidDataTest: TestBase
    {

        #region TestData

        static string[] DivideCasesNames = new string[] { "G","qrSBFoVkahhXYLDRgtcoHWGQdwnyBzxyurXqHcsfXBlfyBILWJl",
        " name","name-","name@","name#","name$","name%","name^","name*","name5" };

        static string[] DivideCasesEmail = new string[] { "fsapf","ya hah@gmail.com","yahahgmail.com", "email@com" };

        #endregion

        private EditSecretaryPage editSecretaryPage;
        private int secretaryID = 5;

        [SetUp]
        public void Set()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            editSecretaryPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>()
                            .EditSecretary(secretaryID) ;
        }

        [TearDown]
        public void Down()
        {
            editSecretaryPage.ClickClearButton();
            editSecretaryPage.Logout();
        }

        [Test]

        [TestCaseSource(nameof(DivideCasesNames))]

        public void InvalidFirstNameTest(string data)
        {
            string expected = WarningMessagesData.WarningMessages(data, WarningMessagesData.FirstName);

            editSecretaryPage.Fill_FirstName(data);

            string actual = editSecretaryPage.DangerFieldMessage(WarningMessagesData.FirstName);


            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.save).Enabled);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.layOff).Enabled);
                Assert.IsTrue(driver.FindElement(editSecretaryPage.clear).Enabled);
            });
        }

        [Test]

        [TestCaseSource(nameof(DivideCasesNames))]

        public void InvalidLastNameTest(string data)
        {
            string expected = WarningMessagesData.WarningMessages(data, WarningMessagesData.LastName);

            editSecretaryPage.Fill_LastName(data);

            string actual = editSecretaryPage.DangerFieldMessage(WarningMessagesData.LastName);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.save).Enabled);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.layOff).Enabled);
                Assert.IsTrue(driver.FindElement(editSecretaryPage.clear).Enabled);
            });
        }

        [Test]

        [TestCaseSource(nameof(DivideCasesEmail))]
        public void InvalidEmailTest(string data)
        {
            string expected = WarningMessagesData.WarningMessages(data, WarningMessagesData.Email);

            editSecretaryPage.Fill_Email(data);

            string actual = editSecretaryPage.DangerFieldMessage(WarningMessagesData.Email);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.save).Enabled);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.layOff).Enabled);
                Assert.IsTrue(driver.FindElement(editSecretaryPage.clear).Enabled);
            });
        }

    }
}
