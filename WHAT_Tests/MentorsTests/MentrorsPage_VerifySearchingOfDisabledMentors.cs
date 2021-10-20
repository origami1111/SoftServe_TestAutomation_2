using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class MentrorsPage_VerifySearchingOfDisabledMentors : TestBase
    {
        WhatAccount mentor;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);

            var unassigned = api.RegistrationUser(newUser);
            mentor = api.AssignRole(unassigned, Role.Mentor);
            api.DisableAccount(mentor, Role.Mentor);

            log.Info($"Go to {driver.Url}");
        }

        [Test, Description("DP213-168")]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void TestMentrorsPage_VerifySearchingOfDisabledMentors(Role role)
        {
            var mentorName = $"{mentor.FirstName} {mentor.LastName}";
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .ClickDisabledMentorsToggle()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(mentorName)
                .VerifyFirstNameAtRow(1, mentor.FirstName)
                .VerifyLastNameAtRow(1, mentor.LastName)
                .VerifyEmailAtRow(1, mentor.Email);
        }
    }
}
