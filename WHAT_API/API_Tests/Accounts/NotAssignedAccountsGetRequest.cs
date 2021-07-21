﻿using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Accounts
{
    [AllureNUnit]
    [TestFixture]
    class NotAssignedAccountsGetRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        private Account expectedData;

        public NotAssignedAccountsGetRequest()
        {
            log = LogManager.GetLogger($"Accounts/{nameof(NotAssignedAccountsGetRequest)}");
        }

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
        [TestCase(HttpStatusCode.OK, Role.Secretary)]
        public void ReturnAllNotRegisteredAccounts(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            request = InitNewRequest("ApiAccountsNotAssigned", Method.GET, authenticator);

            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");

            string json = response.Content;
            var users = JsonConvert.DeserializeObject<List<Account>>(json);
            var actualData = users.Where(user => user.Email == expectedData.Email).FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedData.Email, actualData.Email, "Email");
                Assert.AreEqual(expectedData.FirstName, actualData.FirstName, "First name");
                Assert.AreEqual(expectedData.LastName, actualData.LastName, "Last name");
                Assert.AreEqual(expectedData.Role, actualData.Role, "Role");
                Assert.AreEqual(expectedData.Activity, actualData.Activity, "Is active");
            });
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Forbidden, Role.Mentor)]
        [TestCase(HttpStatusCode.Forbidden, Role.Student)]
        public void ReturnAllNotRegisteredAccountsWithStatusCode403(HttpStatusCode expectedStatusCode, Role role)
        {
            var authenticator = GetAuthenticatorFor(role);
            request = InitNewRequest("ApiAccountsNotAssigned", Method.GET, authenticator);

            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
        }

    }
}