using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class MentorsPage_DisabledMentorsSwitch : TestBase
    {
        WhatAccount enabledMentor;
        WhatAccount disabledMentor;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30); ;
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30); ;

            var unassigned = api.RegistrationUser(newUser);
            enabledMentor = api.AssignRole(unassigned, Role.Mentor);

            newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30); ;
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30); ;

            unassigned = api.RegistrationUser(newUser);
            disabledMentor = api.AssignRole(unassigned, Role.Mentor);
            api.DisableAccount(disabledMentor, Role.Mentor);

            log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(enabledMentor, Role.Mentor);
        }

        [Test, Description("DP213-158")]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void TestDisabledMentorsSwitch(Role role)
        {
            var enabledMentorName = $"{enabledMentor.FirstName} {enabledMentor.LastName}";
            var disabledMentorName = $"{disabledMentor.FirstName} {disabledMentor.LastName}";
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(enabledMentorName)
                .VerifyFirstNameAtRow(1, enabledMentor.FirstName)
                .VerifyLastNameAtRow(1, enabledMentor.LastName)
                .VerifyEmailAtRow(1, enabledMentor.Email)
                .ClickDisabledMentorsToggle()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(disabledMentorName)
                .VerifyFirstNameAtRow(1, disabledMentor.FirstName)
                .VerifyLastNameAtRow(1, disabledMentor.LastName)
                .VerifyEmailAtRow(1, disabledMentor.Email);
        }
    }
}
