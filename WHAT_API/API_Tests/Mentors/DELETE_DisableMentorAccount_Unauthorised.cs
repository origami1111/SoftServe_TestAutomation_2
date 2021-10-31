using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    [AllureNUnit]
    class DELETE_DisableMentorAccount_Unauthorised : API_BaseTest
    {
        WhatAccount mentor;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            var unassigned = api.RegistrationUser(newUser);
            mentor = api.AssignRole(unassigned, Role.Mentor);
        }

        [Test]
        public void VerifyDisableMentorAccount_Unauthorised()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(DELETE_DisableMentorAccount_Unauthorised)}");
            var endpoint = "ApiMentorId";
            var request = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.DELETE);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
        }
    }
}
