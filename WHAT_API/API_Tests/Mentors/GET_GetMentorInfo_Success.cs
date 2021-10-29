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
    [TestFixture(Role.Mentor)]
    [AllureNUnit]
    class GET_GetMentorInfo_Success : API_BaseTest
    {
        WhatAccount mentor;
        WhatAccount infoGetter;
        Credentials infoGetterCredentials;

        Role role;

        public GET_GetMentorInfo_Success(Role role) : base()
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
            mentor = api.AssignRole(mentor, Role.Mentor);

            if (role == Role.Admin)
            {
                infoGetterCredentials = ReaderFileJson.ReadFileJsonCredentials(Role.Admin);
                return;
            }
            var userInfoGetter = new GenerateUser();
            userInfoGetter.FirstName = StringGenerator.GenerateStringOfLetters(30);
            userInfoGetter.LastName = StringGenerator.GenerateStringOfLetters(30);
            infoGetter = api.RegistrationUser(userInfoGetter);
            infoGetter = api.AssignRole(infoGetter, role);
            infoGetterCredentials = new Credentials { Email = userInfoGetter.Email, Password = userInfoGetter.Password, Role = role };
        }

        [Test]
        public void VerifyGetMentorInfo_Success()
        {
            api.log = LogManager.GetLogger($"Mentors/{nameof(GET_GetMentorInfo_Success)}");

            var endpoint = "ApiGetMentorInfo";
            var authenticator = api.GetAuthenticatorFor(infoGetterCredentials);
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
            if (role != Role.Admin)
            {
                api.DisableAccount(infoGetter, role);
            }
            api.DisableAccount(mentor, Role.Mentor);
        }
    }
}
