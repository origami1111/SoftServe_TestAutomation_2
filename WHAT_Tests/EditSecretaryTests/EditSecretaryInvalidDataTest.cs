using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class EditSecretaryInvalidDataTest : TestBase
    {
        #region TestData
        private static IEnumerable<TestCaseData> InvalidFirstNameData()
        {
            yield return new TestCaseData("G", "Too short");

            yield return new TestCaseData("qrSBFoVkahhXYLDRgtcoHWGQdwnyBzxyurXqHcsfXBlfyBILWJl",
                                          "Too long");

            yield return new TestCaseData(" name",
                                          "Invalid first name");

            yield return new TestCaseData("name-", "Invalid first name");

            yield return new TestCaseData("name#", "Invalid first name");
        }
        private static IEnumerable<TestCaseData> InvalidLastNameData()
        {
            yield return new TestCaseData("B", "Too short");

            yield return new TestCaseData("qrSBFoVkahhXYLDRgtcoHWGQdwnyBzxyurXqHcsfXBlfyBILWJl",
                                          "Too long");

            yield return new TestCaseData(" LastName", "Invalid last name");

            yield return new TestCaseData("LastName-", "Invalid last name");

            yield return new TestCaseData("LastName#", "Invalid last name");
        }
        private static IEnumerable<TestCaseData> InvalidEmailData()
        {
            yield return new TestCaseData("fsapf", "Invalid email address");

            yield return new TestCaseData("ya hah@gmail.com", "Invalid email address");

            yield return new TestCaseData("yahahgmail.com", "Invalid email address");

            yield return new TestCaseData("email@com", "Invalid email address");
        }
        #endregion

        private EditSecretaryPage editSecretaryPage;
        private int secretaryID = 5;

        [SetUp]
        public void PreCondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            editSecretaryPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>()
                            .EditSecretary(secretaryID);
        }

        [TearDown]
        public void PostCondition()
        {
            editSecretaryPage.ClickClearButton();
            editSecretaryPage.Logout();
        }

        [Test]
        [TestCaseSource(nameof(InvalidFirstNameData))]
        public void InvalidFirstNameTest(string firstName, string expected)
        {
            editSecretaryPage.Fill_FirstName(firstName);

            string actual = editSecretaryPage.GetFirstNameDangerField();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.save).Enabled);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.layOff).Enabled);
                Assert.IsTrue(driver.FindElement(editSecretaryPage.clear).Enabled);
            });
        }

        [Test]
        [TestCaseSource(nameof(InvalidLastNameData))]
        public void InvalidLastNameTest(string lastName, string expected)
        {
            editSecretaryPage.Fill_LastName(lastName);

            string actual = editSecretaryPage.GetLastNameDangerField();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.save).Enabled);
                Assert.IsFalse(driver.FindElement(editSecretaryPage.layOff).Enabled);
                Assert.IsTrue(driver.FindElement(editSecretaryPage.clear).Enabled);
            });
        }

        [Test]
        [TestCaseSource(nameof(InvalidEmailData))]
        public void InvalidEmailTest(string email, string expected)
        {
            editSecretaryPage.Fill_Email(email);

            string actual = editSecretaryPage.GetEmailDangerField();

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
