using NUnit.Allure.Core;
using NUnit.Framework;
using System.Collections.Generic;
using WHAT_PageObject;
using WHAT_Utilities;

namespace WHAT_Tests
{
    [TestFixture]
    [Category("FrontEndTest-Mentors")]
    [AllureNUnit]
    class MentrorsPage_VerifySearchingOfActiveMentors : TestBase
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

        [Test, Description("DP213-167")]
        [TestCase(Role.Admin)]
        public void TestMentrorsPage_VerifySearchingOfActiveMentors(Role role)
        {
            var mentorName = $"{mentor.FirstName} {mentor.LastName}";
            var testData = GetTestData(mentorName);
            var credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var page = new MentorsPage(driver);
            new SignInPage(driver)
                .SignInAsAdmin(credentials.Email, credentials.Password)
                .SidebarNavigateTo<MentorsPage>()
                .WaitUntilMentorsTableLoads()
                .ForEachEntry<MentorsPage, string>(testData, page, (data) =>
                 {
                     new MentorsPage(driver)
                     .FillSearchField(data)
                     .VerifySearchResults(data);
                 });
        }

        public List<string> GetTestData(string name)
        {
            List<string> testData = new List<string>();
            for (int i = 1; i < name.Length; i++)
            {
                testData.Add(name.Substring(0, i));
            }
            return testData;
        }
    }
}
