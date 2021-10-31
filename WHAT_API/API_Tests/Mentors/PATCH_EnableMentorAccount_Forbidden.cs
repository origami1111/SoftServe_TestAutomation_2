using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Mentor)]
    [TestFixture(Role.Student)]
    [AllureNUnit]
    class PATCH_EnableMentorAccount_Forbidden : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount accountEnabler;
        Credentials accountEnablerCredentials;

        Role role;

        public PATCH_EnableMentorAccount_Forbidden(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            mentor = api.RegistrationUser(newUser);
            mentor = api.AssignRole(mentor, Role.Mentor);
            api.DisableAccount(mentor, Role.Mentor);

            if (role == Role.Admin)
            {
                accountEnablerCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            accountEnabler = api.RegistrationUser(userInfoGetter);
            accountEnabler = api.AssignRole(accountEnabler, role);
            accountEnablerCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyEnableMentorAccount_Forbidden()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(PATCH_EnableMentorAccount_Forbidden)}");

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountEnablerCredentials);
            var request = api.InitNewRequest(endpoint, Method.PATCH, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(accountEnabler, role);
            }
        }
    }
}
