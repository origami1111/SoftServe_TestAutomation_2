using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_API.Entities;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Accounts
{
    [TestFixture]
    class SignInPostRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin, Activity.Active)]
        [TestCase(HttpStatusCode.OK, Role.Mentor, Activity.Active)]
        [TestCase(HttpStatusCode.OK, Role.Secretar, Activity.Active)]
        [TestCase(HttpStatusCode.OK, Role.Student, Activity.Active)]
        public void Test(HttpStatusCode expectedStatusCode, Role role, Activity activity)
        {
            Credentials expectedData = ReaderFileJson.ReadFileJsonCredentials(role, activity);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { expectedData.Email, expectedData.Password });
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string json = response.Content;

            SignInResponseBody actualData = JsonConvert.DeserializeObject<SignInResponseBody>(json);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(expectedData.FirstName == actualData.FirstName 
                    && expectedData.LastName == actualData.LastName
                    && expectedData.Role == actualData.Role);
            });
        }

        [TestCase(HttpStatusCode.Forbidden, "is registered and waiting assign.", Role.Unassigned, Activity.Active)]
        [TestCase(HttpStatusCode.Unauthorized, "Account is not active!", Role.Mentor, Activity.Inactive)]
        public void Test2(HttpStatusCode expectedStatusCode, string expected, Role role, Activity activity)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role, activity);
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string actual = response.Content;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.Contains(expected));
            });
        }
    }
}
