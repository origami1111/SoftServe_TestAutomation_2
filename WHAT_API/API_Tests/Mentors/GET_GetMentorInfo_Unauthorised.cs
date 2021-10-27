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
    class GET_GetMentorInfo_Unauthorised : API_BaseTest
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
        public void VerifyGetMentorInfo_Unauthorised()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorInfo_Unauthorised)}");
            var endpoint = "ApiGetMentorInfo";
            var request = new RestRequest(ReaderUrlsJSON.ByName(endpoint, api.endpointsPath), Method.GET);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
