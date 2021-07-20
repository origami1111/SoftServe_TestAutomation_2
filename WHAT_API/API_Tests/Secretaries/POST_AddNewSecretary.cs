using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using WHAT_Utilities;

namespace WHAT_API.API_Tests
{
    [TestFixture]
    class POST_AddNewSecretary : API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;

        public POST_AddNewSecretary()
        {
            log = LogManager.GetLogger($"Secretaries/{nameof(POST_AddNewSecretary)}");
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyAddingStudentAccount_Valid(Role role)
        {
            ////POST
            //var expectedUser = UserGenerator.GenerateUser();
            //string expectedAvatarUrl = null;
            //request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            //request.AddJsonBody(expectedUser);
            //response = client.Execute(request);
            //log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");

            ////GET
            //request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath), Method.GET);
            //request.AddHeader("Authorization", GetToken(role));
            //response = client.Execute(request);
            //int newUserAccountId = JsonConvert.DeserializeObject<List<RegistrationResponseBody>>(response.Content).Max(s => s.Id); ;
            //log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");
        }
    }       
}

        