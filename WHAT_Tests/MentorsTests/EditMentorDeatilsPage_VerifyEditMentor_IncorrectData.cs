using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class EditMentorDeatilsPage_VerifyEditMentor_IncorrectData : TestBase
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

        [Test, Description("DP213-170")]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void TestEditMentorDeatilsPage_VerifyEditMentor_IncorrectData(Role role)
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var userName = $"{mentor.FirstName} {mentor.LastName}";
            var invalidFirstNames = ReaderFileJson.ReadFileJsonTestData(JsonFiles.EditMentorsInvalidFirstNames);
            var invalidLastNames = ReaderFileJson.ReadFileJsonTestData(JsonFiles.EditMentorsInvalidLastNames);
            var invalidEmails = ReaderFileJson.ReadFileJsonTestData(JsonFiles.EditMentorsInvalidEmails);
            var softAssetions = new SoftAssert();
            var page = new EditMentorDetailsPage(driver);

            new SignInPage(driver)
                .SignInAsSecretar(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(userName)
                .ClickEditMentorButtonOnRow(1)
                .WaitUntilFormLoads()
                .ForEachEntry<EditMentorDetailsPage, TestData>(invalidFirstNames, page, (data) => 
                {
                    new EditMentorDetailsPage(driver)
                    .FillFirstNameField(data.Value)
                    .SoftAssertAdd<EditMentorDetailsPage>(
                        softAssetions,
                        data.Result,
                        page.GetFirstNameError(),
                        AssertionMessages.EditMentorsDetailsPage.FIRST_NAME_ERROR_MESSAGE)
                    .SoftAssertAdd<EditMentorDetailsPage>(
                        softAssetions,
                        false,
                        page.IsSaveButtonEnabled(),
                        AssertionMessages.EditMentorsDetailsPage.SAVE_BUTTON_ENABLED_MESSAGE);
                })
                .ClickResetButton()
                .ForEachEntry<EditMentorDetailsPage, TestData>(invalidLastNames, page, (data) =>
                {
                    new EditMentorDetailsPage(driver)
                    .FillLastNameField(data.Value)
                    .SoftAssertAdd<EditMentorDetailsPage>(
                        softAssetions,
                        data.Result,
                        page.GetLastNameError(),
                        AssertionMessages.EditMentorsDetailsPage.LAST_NAME_ERROR_MESSAGE)
                    .SoftAssertAdd<EditMentorDetailsPage>(
                        softAssetions,
                        false,
                        page.IsSaveButtonEnabled(),
                        AssertionMessages.EditMentorsDetailsPage.SAVE_BUTTON_ENABLED_MESSAGE);
                })
                .ClickResetButton()
                .ForEachEntry<EditMentorDetailsPage, TestData>(invalidEmails, page, (data) =>
                {
                    new EditMentorDetailsPage(driver)
                    .FillEmailField(data.Value)
                    .SoftAssertAdd<EditMentorDetailsPage>(
                        softAssetions,
                        data.Result,
                        page.GetEmailError(),
                        AssertionMessages.EditMentorsDetailsPage.EMAIL_ERROR_MESSAGE)
                    .SoftAssertAdd<EditMentorDetailsPage>(
                        softAssetions,
                        false,
                        page.IsSaveButtonEnabled(),
                        AssertionMessages.EditMentorsDetailsPage.SAVE_BUTTON_ENABLED_MESSAGE);
                })
                .SoftAssertAll<EditMentorDetailsPage>(softAssetions);
        }
    }
}
