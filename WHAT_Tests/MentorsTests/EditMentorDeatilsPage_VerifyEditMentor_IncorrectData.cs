using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [AllureNUnit]
    class EditMentorDeatilsPage_VerifyEditMentor_IncorrectData : TestBase
    {
        [SetUp]
        public void Precondition()
        {
            //log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            new MentorsPage(driver).Logout();
        }

        [Test, Description("DP213-170")]
        [TestCase (Role.Admin)]
        [TestCase (Role.Secretary)]
        public void TestEditMentorDeatilsPage_VerifyEditMentor_IncorrectData(Role role)
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var invalidFirstNames = ReaderFileJson.ReadFileJsonTestData(@"DataFiles\MentorsPage\EditMentorsPageInvalidFirstNames.json");
            var invalidLastNames = ReaderFileJson.ReadFileJsonTestData(@"DataFiles\MentorsPage\EditMentorsPageInvalidLastNames.json");
            var invalidEmails = ReaderFileJson.ReadFileJsonTestData(@"DataFiles\MentorsPage\EditMentorsPageInvalidEmails.json");
            var softAssetions = new SoftAssert();
            var page = new EditMentorDetailsPage(driver);

            new SignInPage(driver)
                .SignInAsSecretar(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
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
                        true,
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

        [Test, Description("DP213-150")]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void TestEditMentorDeatilsPage_VerifyEditMentor_CorrectData(Role role)
        {
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var firstName = StringGenerator.GenerateStringOfLetters(50);
            var lastName = StringGenerator.GenerateStringOfLetters(50);
            var email = $"{Guid.NewGuid():N}@example.com";

            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .ClickEditMentorButtonOnRow(1)
                .WaitUntilFormLoads()
                .FillFirstNameField(firstName)
                .VerifyFirstNameFilled(firstName)
                .FillLastNameField(lastName)
                .VerifyLastNameFilled(lastName)
                .FillEmailField(email)
                .VerifyEmailFilled(email)
                //.ClickSaveButton()
                ;
        }
    }
}
