using Newtonsoft.Json;
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
    class GET_GetMentorCourses_BadRequest : API_BaseTest
    {
        WhatAccount infoGetter;
        Credentials infoGetterCredentials;

        Role role;

        public GET_GetMentorCourses_BadRequest(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
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
        public void VerifyGetMentorCourses_BadRequest()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorCourses_BadRequest)}");
            ulong TooLongtMentorId = ulong.MaxValue;
            var endpoint = "ApiMentorsIdCourses";
            var authenticator = api.GetAuthenticatorFor(infoGetterCredentials);
            var request = api.InitNewRequest(endpoint, Method.GET, authenticator);
            request.AddUrlSegment("id", TooLongtMentorId.ToString());
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
