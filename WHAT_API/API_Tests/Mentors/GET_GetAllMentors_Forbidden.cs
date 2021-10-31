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
    [TestFixture(Role.Mentor)]
    [TestFixture(Role.Student)]
    [AllureNUnit]
    class GET_GetAllMentors_Forbidden : API_BaseTest
    {
        WhatAccount infoGetter;
        Credentials infoGetterCredentials;

        Role role;

        public GET_GetAllMentors_Forbidden(Role role) : base()
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
        public void VerifyGetAllMentors_Forbidden()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetAllMentors_Forbidden)}");

            var endpoint = "ApiAllMentors";
            var authenticator = api.GetAuthenticatorFor(infoGetterCredentials);
            var request = api.InitNewRequest(endpoint, Method.GET, authenticator);
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
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
