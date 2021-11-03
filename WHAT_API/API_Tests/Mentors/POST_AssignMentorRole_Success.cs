using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture(Role.Admin)]
    [TestFixture(Role.Secretary)]
    [Category("ApiTest-Mentors")]
    [AllureNUnit]
    class POST_AssignMentorRole_Success : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount assigner;
        Credentials assignerCredentials;

        Role role;

        public POST_AssignMentorRole_Success(Role role) : base()
        {
            this.role = role;
        }

        [SetUp]
        public void Precondition()
        {
            var newUser = new GenerateUser();
            newUser.FirstName = StringGenerator.GenerateStringOfLetters(30);
            newUser.LastName = StringGenerator.GenerateStringOfLetters(30);
            mentor = api.RegistrationUser(newUser);

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
        public void VerifyAssignMentorRole_Success()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(POST_AssignMentorRole_Success)}");

            var endpoint = "ApiMentorId";
            var authenticator = api.GetAuthenticatorFor(assignerCredentials);
            var request = api.InitNewRequest(endpoint, Method.POST, authenticator);
            request.AddUrlSegment("accountId", mentor.Id.ToString());
            IRestResponse response = APIClient.client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string contentJson = response.Content;
            var userInfo = JsonConvert.DeserializeObject<WhatAccount>(contentJson);
            Assert.Multiple(() =>
            {                
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
