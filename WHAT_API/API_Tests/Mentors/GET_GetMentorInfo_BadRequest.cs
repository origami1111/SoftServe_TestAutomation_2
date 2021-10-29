using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Admin)]
    [AllureNUnit]
    class GET_GetMentorInfo_BadRequest : API_BaseTest
    {
        WhatAccount infoGetter;
        Credentials infoGetterCredentials;

        Role role;

        public GET_GetMentorInfo_BadRequest(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            if (role == Role.Admin)
            {
                infoGetterCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            infoGetter = api.RegistrationUser(userInfoGetter);
            infoGetter = api.AssignRole(infoGetter, role);
            infoGetterCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyGetMentorInfo_BadRequest()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorInfo_BadRequest)}");
            ulong TooLongtMentorId = ulong.MaxValue;

            var endpoint = "ApiGetMentorInfo";
            var authenticator = api.GetAuthenticatorFor(infoGetterCredentials);
            var request = api.InitNewRequest(endpoint, Method.GET, authenticator);
            request.AddUrlSegment("accountId", TooLongtMentorId.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);            
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(infoGetter, role);
            }
        }
    }
}
