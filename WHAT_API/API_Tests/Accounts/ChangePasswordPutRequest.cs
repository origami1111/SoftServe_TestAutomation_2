using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_API.Entities;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Accounts
{
    [TestFixture]
    class ChangePasswordPutRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private RegistrationResponseBody registeredUser;
        private ChangePasswordRequestBody changePasswordRequestBody;

        /// <summary>
        /// 1. Create account by POST method
        /// 2. Get the id of the last unassigned user
        /// 3. Assign account to ROLE(mentor)
        /// 4. Log in
        /// 5. Change password
        /// 6. Verify that user can log in the system using new password
        /// </summary>

        [OneTimeSetUp]
        public void PreCondition()
        {
            var authenticator = GetAuthenticatorFor(Role.Admin);

            // 1
            registeredUser = RegistrationUser();

            // 2
            request = InitNewRequest("ApiAccountsNotAssigned", Method.GET, authenticator);
            response = client.Execute(request);

            string json = response.Content;
            var users = JsonConvert.DeserializeObject<List<RegistrationResponseBody>>(json);
            var searchedUser = users.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            registeredUser.Id = searchedUser.Id;

            // 3
            request = InitNewRequest("ApiMentorsAssignAccountToMentor-accountID", Method.POST, authenticator);
            request.AddUrlSegment("accountID", registeredUser.Id.ToString());
            response = client.Execute(request);

            registeredUser.Role = Role.Mentor;

            changePasswordRequestBody = new ChangePasswordRequestBody()
            {
                Email = registeredUser.Email,
                CurrentPassword = "Qwerty_123",
                NewPassword = "Qwerty_1234",
                ConfirmPassword = "Qwerty_1234"
            };
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        public void Test(HttpStatusCode expectedStatusCode)
        {
            // 4
            var authenticator = GetAuthenticatorFor(Role.Admin);

            // 5
            request = InitNewRequest("ApiAccountsChangePassword", Method.PUT, authenticator);
            request.AddJsonBody(changePasswordRequestBody);
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string json = response.Content;
            var actualDataChangePassword = JsonConvert.DeserializeObject<RegistrationResponseBody>(json);

            Assert.AreEqual(registeredUser, actualDataChangePassword);

            // 6
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { changePasswordRequestBody.Email, changePasswordRequestBody.NewPassword });
            response = client.Execute(request);

            actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            json = response.Content;
            SignInResponseBody actualData = JsonConvert.DeserializeObject<SignInResponseBody>(json);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualDataChangePassword.FirstName == actualData.FirstName
                    && actualDataChangePassword.LastName == actualData.LastName
                    && actualDataChangePassword.Role == actualData.Role);
            });
        }
    }
}
