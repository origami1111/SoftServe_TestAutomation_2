using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Student)]
    [AllureNUnit]
    class GET_GetMentorInfo_Forbidden : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount infoGetter;
        Credentials infoGetterCredentials;
        Role role;

        public GET_GetMentorInfo_Forbidden(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            var unassigned = api.RegistrationUser(newUser);
            mentor = api.AssignRole(unassigned, Role.Mentor);

            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            infoGetter = api.RegistrationUser(userInfoGetter);
            infoGetter = api.AssignRole(infoGetter, role);
            infoGetterCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyAssignMentorRole_Invalid()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorInfo_Forbidden)}");
            var endpoint = "ApiGetMentorInfo";
            var authenticator = api.GetAuthenticatorFor(infoGetterCredentials);
            var request = api.InitNewRequest(endpoint, Method.POST, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse assignRoleResponse = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Forbidden, assignRoleResponse.StatusCode);            
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(infoGetter, role);
            api.DisableAccount(mentor, Role.Mentor);
        }
    }
}
