using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Accounts
{
    [TestFixture]
    class AllRegisteredAccountsGetRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private RegistrationResponseBody expectedData;

        /// <summary>
        /// Create account by POST method
        /// </summary>
        [OneTimeSetUp]
        public void PreCondition()
        {
            expectedData = RegistrationUser();
        }

        [Test]
        [TestCase(HttpStatusCode.OK, Role.Admin)]
        public void ReturnAllRegisteredAccounts(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            request = InitNewRequest("ApiAccountsAll", Method.GET, authenticator);

            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string json = response.Content;
            var users = JsonConvert.DeserializeObject<List<RegistrationResponseBody>>(json);
            var actualData = users.Where(user => user.Email == expectedData.Email).FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedData, actualData);
            });
        }

        [Test]
        [TestCase(HttpStatusCode.Forbidden, Role.Secretary)]
        [TestCase(HttpStatusCode.Forbidden, Role.Mentor)]
        [TestCase(HttpStatusCode.Forbidden, Role.Student)]
        public void ReturnAllRegisteredAccountsWithStatusCode403(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            request = InitNewRequest("ApiAccountsAll", Method.GET, authenticator);

            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

    }
}
