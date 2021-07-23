using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    public class EditSecretaryValidDataTest : TestBase
    {
        private EditSecretaryPage editSecretaryPage;
        private UnassignedUsersPage findUser;
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private int ID { get; set; } = 3;

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

        [SetUp]
        public void PreCondition()
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);

            editSecretaryPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
                            .SidebarNavigateTo<SecretariesPage>()
                            .EditSecretary(ID);

            FirstName = editSecretaryPage.GetFirstName();
            LastName = editSecretaryPage.GetLastName();
            Email = editSecretaryPage.GetEmail();
        }

        [TearDown]
        public void PostCondition()
        {
            editSecretaryPage.SidebarNavigateTo<SecretariesPage>()
                             .EditSecretary(ID)
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
            bool actual = findUser.UserVerify<SecretariesPage>(firstName, lastName, email);

            Assert.IsTrue(actual);
        }
    }
}

