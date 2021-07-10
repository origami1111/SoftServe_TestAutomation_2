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
        [TestCase(HttpStatusCode.OK, Role.Admin)]
        [TestCase(HttpStatusCode.OK, Role.Mentor)]
        [TestCase(HttpStatusCode.OK, Role.Secretar)]
        [TestCase(HttpStatusCode.OK, Role.Student)]
        [TestCase(HttpStatusCode.OK, Role.Unassigned)]

        public void Test(HttpStatusCode expectedStatusCode, Role role)
        {
            Credentials expectedData = ReaderFileJson.ReadFileJsonCredentials(role);
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
                    && expectedData.Role == actualData.Role
                    && expectedData.ID == actualData.ID);
            });
        }
    }
}
