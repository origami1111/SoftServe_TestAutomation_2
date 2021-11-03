using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Admin)]
    [Category("ApiTest-Mentors")]
    [AllureNUnit]
    class PUT_UpdateMentorAccount_NotFound : API_BaseTest
    {
        WhatAccount accountUpdater;
        Credentials accountUpdaterCredentials;

        Role role;

        public PUT_UpdateMentorAccount_NotFound(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            if (role == Role.Admin)
            {
                accountUpdaterCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            accountUpdater = api.RegistrationUser(userInfoGetter);
            accountUpdater = api.AssignRole(accountUpdater, role);
            accountUpdaterCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyUpdateMentorAccount_NotFound()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(PUT_UpdateMentorAccount_NotFound)}");
            var newMentorInfo = new UpdateMentorDto()
            {
                FirstName = StringGenerator.GenerateStringOfLetters(30),
                LastName = StringGenerator.GenerateStringOfLetters(30),
                Email = StringGenerator.GenerateEmail()
            };
            long NonExistantMentorId = long.MaxValue;

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountUpdaterCredentials);
            var request = api.InitNewRequest(endpoint, Method.PUT, authenticator);
            request.AddUrlSegment("accountId", NonExistantMentorId.ToString());
            request.AddJsonBody(newMentorInfo);
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(accountUpdater, role);
            }
        }
    }
}
