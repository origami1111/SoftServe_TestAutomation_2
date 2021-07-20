using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_API.Entities;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Accounts
{
    [AllureNUnit]
    [TestFixture]
    class ChangePasswordPutRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private Account registeredUser;
        private ChangeCurrentPassword changePasswordRequestBody;

        public ChangePasswordPutRequest()
        {
            log = LogManager.GetLogger($"Accounts/{nameof(ChangePasswordPutRequest)}");
        }

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
            var users = JsonConvert.DeserializeObject<List<Account>>(json);
            var searchedUser = users.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            registeredUser.Id = searchedUser.Id;

            // 3
            request = InitNewRequest("ApiMentorsAssignAccountToMentor-accountID", Method.POST, authenticator);
            request.AddUrlSegment("accountId", registeredUser.Id.ToString());
            response = client.Execute(request);

            registeredUser.Role = Role.Mentor;

            changePasswordRequestBody = new ChangeCurrentPassword()
            {
                Email = registeredUser.Email,
                CurrentPassword = "Qwerty_123",
                NewPassword = "Qwerty_1234",
                ConfirmNewPassword = "Qwerty_1234"
            };
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        public void ChangePasswordWithStatusCode200(HttpStatusCode expectedStatusCode)
        {
            // 4
            var authenticator = GetAuthenticatorFor(Role.Admin);

            // 5
            request = InitNewRequest("ApiAccountsChangePassword", Method.PUT, authenticator);
            request.AddJsonBody(changePasswordRequestBody);

            log.Info($"PUT request to {ReaderUrlsJSON.ByName("ApiAccountsChangePassword", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string json = response.Content;
            Account actualDataChangePassword = JsonConvert.DeserializeObject<Account>(json);

            Assert.AreEqual(registeredUser, actualDataChangePassword);
            log.Info($"Expected and actual results is checked");

            // 6
            Authentication signInRequestBody = new Authentication()
            {
                Email = changePasswordRequestBody.Email,
                Password = changePasswordRequestBody.NewPassword
            };

            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(signInRequestBody);
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
            log.Info($"Expected and actual results is checked");
        }

    }
}
