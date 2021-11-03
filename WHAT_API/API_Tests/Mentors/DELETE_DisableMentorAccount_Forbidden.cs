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
    [Category("ApiTest-Mentors")]
    [AllureNUnit]
    class DELETE_DisableMentorAccount_Forbidden : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount accountDeactivator;
        Credentials accountDeactivatorCredentials;
        Role role;

        public DELETE_DisableMentorAccount_Forbidden(Role role) : base()
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
            accountDeactivator = api.RegistrationUser(userInfoGetter);
            accountDeactivator = api.AssignRole(accountDeactivator, role);
            accountDeactivatorCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyDisableMentorAccount_Forbidden()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(DELETE_DisableMentorAccount_Forbidden)}");
            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(accountDeactivatorCredentials);
            var request = api.InitNewRequest(endpoint, Method.DELETE, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse assignRoleResponse = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Forbidden, assignRoleResponse.StatusCode);            
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(accountDeactivator, role);
            api.DisableAccount(mentor, Role.Mentor);
        }
    }
}
