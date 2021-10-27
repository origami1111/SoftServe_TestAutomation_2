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
    class POST_AssignMentorRole_Forbidden : API_BaseTest
    {
        WhatAccount unassigned;
        WhatAccount assigner;
        Credentials assignerCredentials;
        Role role;

        public POST_AssignMentorRole_Forbidden(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            unassigned = api.RegistrationUser(newUser);

            var newAssigner = new GenerateUser();
            newAssigner.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newAssigner.LastName = StringGenerator.GenerateStringOfLetters(30);
            assigner = api.RegistrationUser(newAssigner);
            assigner = api.AssignRole(assigner, Role.Mentor);
            assignerCredentials = new Credentials { Email = newAssigner.Email, Password = newAssigner.Password, Role = role };
        }

        [Test]
        public void VerifyAssignMentorRole_Invalid()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(POST_AssignMentorRole_Forbidden)}");
            var endpoint = "ApiMentorsAssignAccountToMentor-accountID";
            var authenticator = api.GetAuthenticatorFor(assignerCredentials);
            var request = api.InitNewRequest(endpoint, Method.POST, authenticator);
            request.AddUrlSegment("accountId", unassigned.Id.ToString());
            IRestResponse assignRoleResponse = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Forbidden, assignRoleResponse.StatusCode);            
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(assigner, role);
        }
    }
}
