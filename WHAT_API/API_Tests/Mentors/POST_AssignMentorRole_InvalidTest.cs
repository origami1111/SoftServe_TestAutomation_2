using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Mentor)]
    [TestFixture(Role.Student)]
    [AllureNUnit]
    class POST_AssignMentorRole_InvalidTest : API_BaseTest
    {
        WhatAccount unassigned;
        WhatAccount assigner;
        Credentials assignerCredentials;
        Role role;

        public POST_AssignMentorRole_InvalidTest(Role role) : base()
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
            log = LogManager.GetLogger($"Mentors/{nameof(POST_AssignMentorRole_InvalidTest)}");
            var endpoint = "ApiMentorsAssignAccountToMentor-accountID";
            var adminAuthenticator = api.GetAuthenticatorFor(assignerCredentials);
            var assignRoleRequest = api.InitNewRequest(endpoint, Method.POST, adminAuthenticator);
            assignRoleRequest.AddUrlSegment("accountId", unassigned.Id.ToString());
            IRestResponse assignRoleResponse = APIClient.client.Execute(assignRoleRequest);
            Assert.AreEqual(HttpStatusCode.Forbidden, assignRoleResponse.StatusCode);            
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(assigner, role);
        }
    }
}
