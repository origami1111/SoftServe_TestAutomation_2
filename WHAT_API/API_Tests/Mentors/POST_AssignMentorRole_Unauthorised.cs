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
    class POST_AssignMentorRole_Unauthorised : API_BaseTest
    {
        WhatAccount unassigned;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            unassigned = api.RegistrationUser(newUser);           
        }

        [Test]
        public void VerifyAssignMentorRole_Unauthorised()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(POST_AssignMentorRole_Unauthorised)}");

            var endpoint = "ApiMentorId";
            var request = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.POST);
            request.AddUrlSegment("accountId", unassigned.Id.ToString());
            IRestResponse assignRoleResponse = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, assignRoleResponse.StatusCode);
        }
    }
}
