using NUnit.Allure.Core;
using NUnit.Framework;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [Category("FrontEndTest-Mentors")]
    [AllureNUnit]
    class MentorDetailsPage_VerifyViewingOfMentorDetails : TestBase
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

            log.Info($"Go to {driver.Url}");
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
        }

        [Test, Description("DP213-147")]
        [TestCase(Role.Admin)]
        public void TestMentorsTablePage_VerifySortingOfActiveMentors(Role role)
        {
            var mentorName = $"{mentor.FirstName} {mentor.LastName}";
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var softAssetions = new SoftAssert();
            var page = new MentorDetailsPage(driver);
            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .FillSearchField(mentorName)
                .ClickMentorNameOnRow(1)
                .WaitUntilMentorDetailsLoads()
                .SoftAssertAdd<MentorDetailsPage>(
                        softAssetions,
                        mentor.FirstName,
                        page.GetFirstName(),
                        AssertionMessages.MentorDetailsPage.FIRST_NAME)
                .SoftAssertAdd<MentorDetailsPage>(
                        softAssetions,
                        mentor.LastName,
                        page.GetLastName(),
                        AssertionMessages.MentorDetailsPage.LAST_NAME)
                .SoftAssertAdd<MentorDetailsPage>(
                        softAssetions,
                        mentor.Email,
                        page.GetEmail(),
                        AssertionMessages.MentorDetailsPage.EMAIL)
                .SoftAssertAll<MentorDetailsPage>(softAssetions);
        }
    }
}
