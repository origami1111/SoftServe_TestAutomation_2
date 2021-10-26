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
    class RegistrationOfAccountPostRequest : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public RegistrationOfAccountPostRequest()
        {
            api.log = LogManager.GetLogger($"Accounts/{nameof(RegistrationOfAccountPostRequest)}");
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        public void RegistrationOfAccountWithStatusCode200(HttpStatusCode expectedStatusCode)
        {
            var expectedData = new GenerateUser();
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(expectedData);

            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", api.endpointsPath)}");
            response = api.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");

            Account actualData = JsonConvert.DeserializeObject<Account>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedData.FirstName, actualData.FirstName, "First name");
                Assert.AreEqual(expectedData.LastName, actualData.LastName, "Last name");
                Assert.AreEqual(expectedData.Email, actualData.Email, "Email");
                Assert.AreEqual(actualData.Role, Role.Unassigned, "Role");
                Assert.AreEqual(actualData.Activity, Activity.Active, "Is active");
            });
            api.log.Info($"Expected and actual results is checked");
        }

        [Test]
        [TestCase(HttpStatusCode.Conflict, "admin.@gmail.com")]
        public void RegistrationOfAccountWithEmailAlreadyExists(HttpStatusCode expectedStatusCode, string email)
        {
            var expectedData = new GenerateUser
            {
                Email = email
            };
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(expectedData);

            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", api.endpointsPath)}");
            response = api.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
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
            var data = new CreateAccount 
            { 
                Email = email, 
                FirstName = firstName, 
                LastName = lastName, 
                Password = password, 
                ConfirmPassword = confirmPassword 
            };
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", api.endpointsPath), Method.POST);
            request.AddJsonBody(data);

            api.log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", api.endpointsPath)}");
            response = api.Execute(request);

            HttpStatusCode actualStatusCode = response.StatusCode;
            api.log.Info($"Request is done with StatusCode: {actualStatusCode}, expected was: {expectedStatusCode}");

            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status code");
        }

    }
}
