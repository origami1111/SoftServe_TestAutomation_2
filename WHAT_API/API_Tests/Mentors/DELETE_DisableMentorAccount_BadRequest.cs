using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Admin)]
    [Category("ApiTest-Mentors")]
    [AllureNUnit]
    class DELETE_DisableMentorAccount_BadRequest : API_BaseTest
    {
        WhatAccount accountDeactivator;
        Credentials accountDeactivatorCredentials;

        Role role;

        public DELETE_DisableMentorAccount_BadRequest(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
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
        public void VerifyDisableMentorAccount_BadRequest()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(DELETE_DisableMentorAccount_BadRequest)}");
            ulong TooLongtMentorId = ulong.MaxValue;

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountDeactivatorCredentials);
            var request = api.InitNewRequest(endpoint, Method.DELETE, authenticator);
            request.AddUrlSegment("accountId", TooLongtMentorId.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);            
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
