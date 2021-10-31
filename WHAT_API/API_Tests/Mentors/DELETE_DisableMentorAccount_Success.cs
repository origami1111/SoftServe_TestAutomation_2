using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Admin)]
    [TestFixture(Role.Secretary)]
    [AllureNUnit]
    class DELETE_DisableMentorAccount_Success : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount accountDeactivator;
        Credentials accountDeactivatorCredentials;

        Role role;

        public DELETE_DisableMentorAccount_Success(Role role) : base()
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

            if (role == Role.Admin)
            {
                accountDeactivatorCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            accountDeactivator = api.RegistrationUser(userInfoGetter);
            accountDeactivator = api.AssignRole(accountDeactivator, role);
            accountDeactivatorCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyDisableMentorAccount_Success()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(DELETE_DisableMentorAccount_Success)}");

            var expectedContent = "true";
            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountDeactivatorCredentials);
            var request = api.InitNewRequest(endpoint, Method.DELETE, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string contentJson = response.Content;
            Assert.AreEqual(expectedContent, contentJson);
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(accountDeactivator, role);
            }
        }
    }
}
