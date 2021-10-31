using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]    
    class EditMentorDeatilsPage_VerifyEditMentor_CorrectData : TestBase
    {
        WhatAccount mentor;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30); ;
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30); ;

            var unassigned = api.RegistrationUser(newUser);
            mentor = api.AssignRole(unassigned, Role.Mentor);

            log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
        }

        [Test, Description("DP213-150")]
        [TestCase(Role.Admin)]
        public void TestEditMentorDeatilsPage_VerifyEditMentor_CorrectData(Role role)
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var userName = $"{mentor.FirstName} {mentor.LastName}";            
            var changedFirstName = StringGenerator.GenerateStringOfLetters(30);
            var changedLastName = StringGenerator.GenerateStringOfLetters(30);
            var newUserName = $"{changedFirstName} {changedLastName}";
            var changedEmail = StringGenerator.GenerateEmail();

            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(userName)
                .ClickEditMentorButtonOnRow(1)
                .WaitUntilFormLoads()
                .FillFirstNameField(changedFirstName)
                .VerifyFirstNameFilled(changedFirstName)
                .FillLastNameField(changedLastName)
                .VerifyLastNameFilled(changedLastName)
                .FillEmailField(changedEmail)
                .VerifyEmailFilled(changedEmail)
                .ClickSaveButton()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(newUserName)
                .ClickEditMentorButtonOnRow(1)
                .WaitUntilFormLoads()
                .VerifyFirstNameFilled(changedFirstName)
                .VerifyLastNameFilled(changedLastName)
                .VerifyEmailFilled(changedEmail);
        }
    }
}
