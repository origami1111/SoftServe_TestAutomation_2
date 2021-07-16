﻿using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Net;
using WHAT_API.Entities;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Accounts
{
    [TestFixture]
    class RegistrationOfAccountPostRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        [Test]
        [TestCase(HttpStatusCode.OK)]
        public void RegistrationOfAccountWithStatusCode200(HttpStatusCode expectedStatusCode)
        {
            var expectedData = UserGenerator.GenerateUser();
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(expectedData);

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");

            Assert.AreEqual(expectedStatusCode, actualStatusCode);

            string json = response.Content;
            RegistrationResponseBody actualData = JsonConvert.DeserializeObject<RegistrationResponseBody>(json);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(expectedData.FirstName == actualData.FirstName
                    && expectedData.LastName == actualData.LastName
                    && expectedData.Email == actualData.Email
                    && actualData.Role == Role.Unassigned
                    && actualData.Activity == Activity.Active);
            });
            log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Conflict, "admin.@gmail.com")]
        public void RegistrationOfAccountWithEmailAlreadyExists(HttpStatusCode expectedStatusCode, string email)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(new 
            {
                email = email,
                firstName = "Test",
                lastName = "Registration",
                password = "Qwerty_123",
                confirmPassword = "Qwerty_123"
            });

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        [TestCase(HttpStatusCode.BadRequest, "", "Test", "Registration", "Qwerty_123", "Qwerty_123")]
        [TestCase(HttpStatusCode.BadRequest, "email@gmail.com", "", "Registration", "Qwerty_123", "Qwerty_123")]
        [TestCase(HttpStatusCode.BadRequest, "email@gmail.com", "Test", "", "Qwerty_123", "Qwerty_123")]
        [TestCase(HttpStatusCode.BadRequest, "email@gmail.com", "Test", "Registration", "", "Qwerty_123")]
        [TestCase(HttpStatusCode.BadRequest, "email@gmail.com", "Test", "Registration", "Qwerty_123", "")]
        public void RegistrationOfAccountWithInvalidData(HttpStatusCode expectedStatusCode, string email, 
            string firstName, string lastName, string password, string confirmPassword)
        {
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(new
            {
                email = email,
                firstName = firstName,
                lastName = lastName,
                password = password,
                confirmPassword = confirmPassword
            });

            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            response = client.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

    }
}