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
    class POST_AssignMentorRole_BadRequest : API_BaseTest
    {
        WhatAccount assigner;
        Credentials assignerCredentials;

        Role role;

        public POST_AssignMentorRole_BadRequest(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            if (role == Role.Admin)
            {
                assignerCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userAssigner = new GenerateUser();
            userAssigner.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userAssigner.LastName = StringGenerator.GenerateStringOfLetters(30);
            assigner = api.RegistrationUser(userAssigner);
            assigner = api.AssignRole(assigner, role);
            assignerCredentials = new Credentials { Email = userAssigner.Email, Password = userAssigner.Password, Role = role };
        }

        [Test]        
        public void VerifyAssignMentorRole_BadRequest()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(POST_AssignMentorRole_BadRequest)}");
            ulong TooLongtMentorId = ulong.MaxValue;

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(assignerCredentials);
            var request = api.InitNewRequest(endpoint, Method.POST, authenticator);
            request.AddUrlSegment("accountId", TooLongtMentorId.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TearDown]
        public void Postcondition()
        {
            if (role != Role.Admin)
            {
                api.DisableAccount(assigner, role);
            }
        }
    }
}
