using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class InvalidDataTest
    {
        private IWebDriver driver;

        #region TestData

        static string[] DivideCasesNames = new string[] { "G","qrSBFoVkahhXYLDRgtcoHWGQdwnyBzxyurXqHcsfXBlfyBILWJl",
        " name","name-","name@","name#","name$","name%","name^","name*","name5" };

        static string[] DivideCasesEmail = new string[] { "fsapf",
        "yahah@gmail.com ","ya hah@gmail.com"," yahah@gmail.com" };

        #endregion

        // private SecretaryPage secretaryPage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
           


        }

        [OneTimeTearDown]
        public void TearDown()
        {
            //   secretaryPage.Logout();
            driver.Quit();
        }

        [Test]

        [TestCaseSource(nameof(DivideCasesNames))]

        public void InvalidFirstNameTest(string data)
        {

            string expected = WarningMessagesData.WarningMessages(data, WarningMessagesData.FirstName);

            //string actual = Fill_FirstName(data);

            //Assert.AreEqual(expected, actual);

        }

        [Test]

        [TestCaseSource(nameof(DivideCasesNames))]

        public void InvalidLastNameTest(string data)
        {

            string expected = WarningMessagesData.WarningMessages(data, WarningMessagesData.LastName);

            //string actual = Fill_LastName(data);

            //Assert.AreEqual(expected, actual);

        }

    }
}
