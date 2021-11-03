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
    class PUT_UpdateMentorAccount_BadRequest : API_BaseTest
    {
        WhatAccount accountUpdater;
        Credentials accountUpdaterCredentials;

        Role role;

        public PUT_UpdateMentorAccount_BadRequest(Role role) : base()
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
        public void VerifyUpdateMentorAccount_BadRequest()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(PUT_UpdateMentorAccount_BadRequest)}");
            var newMentorInfo = new UpdateMentorDto()
            {
                FirstName = StringGenerator.GenerateStringOfLetters(30),
                LastName = StringGenerator.GenerateStringOfLetters(30),
                Email = StringGenerator.GenerateEmail()
            };
            ulong TooLongtMentorId = ulong.MaxValue;

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountUpdaterCredentials);
            var request = api.InitNewRequest(endpoint, Method.PUT, authenticator);
            request.AddUrlSegment("accountId", TooLongtMentorId.ToString());
            request.AddJsonBody(newMentorInfo);
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
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
