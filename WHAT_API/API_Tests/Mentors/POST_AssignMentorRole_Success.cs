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
    class POST_AssignMentorRole_Success : API_BaseTest
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
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyAssignMentorRole_Success(Role role)
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(POST_AssignMentorRole_Success)}");

            var endpoint = "ApiMentorsAssignAccountToMentor-accountID";
            var adminAuthenticator = api.GetAuthenticatorFor(role);
            var assignRoleRequest = api.InitNewRequest(endpoint, Method.POST, adminAuthenticator);
            assignRoleRequest.AddUrlSegment("accountId", unassigned.Id.ToString());
            IRestResponse assignRoleResponse = APIClient.client.Execute(assignRoleRequest);
            string assignJson = assignRoleResponse.Content;
            mentor = JsonConvert.DeserializeObject<WhatAccount>(assignJson);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, assignRoleResponse.StatusCode);
                Assert.AreEqual(unassigned.FirstName, mentor.FirstName);
                Assert.AreEqual(unassigned.LastName, mentor.LastName);
                Assert.AreEqual(unassigned.Email, mentor.Email);
            });
        }

        [TearDown]
        public void Postcondition()
        {
            api.DisableAccount(mentor, Role.Mentor);
        }
    }
}
