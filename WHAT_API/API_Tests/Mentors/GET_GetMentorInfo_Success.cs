using Newtonsoft.Json;
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
    class GET_GetMentorInfo_Success : API_BaseTest
    {
        WhatAccount unassigned;
        WhatAccount mentor;

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            unassigned = api.RegistrationUser(newUser);
            mentor = api.AssignRole(unassigned, Role.Mentor);
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        [TestCase(Role.Mentor)]
        public void VerifyGetMentorInfo_Success(Role role)
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorInfo_Success)}");

            var endpoint = "ApiGetMentorInfo";
            var authenticator = api.GetAuthenticatorFor(role);
            var request = api.InitNewRequest(endpoint, Method.GET, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string contentJson = response.Content;
            var userInfo = JsonConvert.DeserializeObject<WhatAccount>(contentJson);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(mentor.Id, userInfo.Id);
                Assert.AreEqual(mentor.FirstName, userInfo.FirstName);
                Assert.AreEqual(mentor.LastName, userInfo.LastName);
                Assert.AreEqual(mentor.Email, userInfo.Email);
            });
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
        }
    }
}
