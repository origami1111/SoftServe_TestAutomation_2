using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_API.Entities;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    class SignInPostRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public SignInPostRequest()
        {
            log = LogManager.GetLogger($"Accounts/{nameof(SignInPostRequest)}");
        }

        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin, Activity.Active)]
        [TestCase(HttpStatusCode.OK, Role.Mentor, Activity.Active)]
        [TestCase(HttpStatusCode.OK, Role.Secretary, Activity.Active)]
        [TestCase(HttpStatusCode.OK, Role.Student, Activity.Active)]
        public void SignInWithStatusCode200(HttpStatusCode expectedStatusCode, Role role, Activity activity)
        {
            Credentials expectedData = ReaderFileJson.ReadFileJsonCredentials(role, activity);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { expectedData.Email, expectedData.Password });

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");

            string json = response.Content;
            SignInResponseBody actualData = JsonConvert.DeserializeObject<SignInResponseBody>(json);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedData.FirstName, actualData.FirstName, "First name");
                Assert.AreEqual(expectedData.LastName, actualData.LastName, "Last name");
                Assert.AreEqual(expectedData.Role, actualData.Role, "Role");
            });
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Forbidden, "is registered and waiting assign.", Role.Unassigned, Activity.Active)]
        public void SignInWithStatusCodeForbidden(HttpStatusCode expectedStatusCode, string expected, Role role, Activity activity)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role, activity);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");

            string actual = response.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual($"\"{credentials.Email} {expected}\"", actual);
            });
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Unauthorized, "\"Account is not active!\"", Role.Mentor, Activity.Inactive)]
        public void SignInWithStatusCodeUnauthorized(HttpStatusCode expectedStatusCode, string expected, Role role, Activity activity)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role, activity);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");

            string actual = response.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
            });
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.BadRequest, "", "")]
        [TestCase(HttpStatusCode.BadRequest, "admin@gmail.com", "")]
        [TestCase(HttpStatusCode.BadRequest, "", "Qwerty_123")]
        [TestCase(HttpStatusCode.Unauthorized, "admin@gmail.com", "Qwerty_123")]
        public void SignInWithInvalidData(HttpStatusCode expectedStatusCode, string email, string password)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { email, password });

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
        }

    }
}
