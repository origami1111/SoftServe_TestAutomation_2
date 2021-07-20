﻿using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [TestFixture]
    public class DELETE_DisableStudentAccount:API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        public DELETE_DisableStudentAccount()
        {
            log = LogManager.GetLogger($"Students/{nameof(DELETE_DisableStudentAccount)}");
        }

        public void Precondition(Role role)
        {
            var expectedUser = new GenerateUser();
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsReg", endpointsPath), Method.POST);
            request.AddJsonBody(expectedUser);
            response = client.Execute(request);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath)}");
            //
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            response = client.Execute(request);
            int newUserAccountId = JsonConvert.DeserializeObject<List<RegistrationResponseBody>>(response.Content).Max(s => s.Id);
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath)}");
            //
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsAccountId", endpointsPath), Method.POST);
            request = InitNewRequest("ApiStudentsAccountId", Method.POST, GetAuthenticatorFor(role));
            request.AddUrlSegment("accountId", newUserAccountId.ToString());
            request.AddParameter("accountId", newUserAccountId);
            response = client.Execute(request);
            log.Info($"POST request to {response.ResponseUri}");
        }

        [Test]
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void VerifyDeletingStudentAccount_Valid(Role role)
        {
            Precondition(role);
            int lastUserId = GetActiveStudentsList(role).Last().Id;
            int expect = GetActiveStudentsList(role).Count-1;
            log.Info($"List of students is taken, there are {GetActiveStudentsList(role).Count} active students");
            //
            request = InitNewRequest("ApiStudentsId", Method.DELETE, GetAuthenticatorFor(role));
            request.AddUrlSegment("id", lastUserId.ToString());
            response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //
            log.Info($"Deleted students with max id : {lastUserId}");
            int actual = GetActiveStudentsList(role).Count;
            log.Info($"List of students is taken, there are {GetActiveStudentsList(role).Count} active students");
            Assert.AreEqual(expect, actual);
        }
        private List<StudentResponseBody> GetActiveStudentsList(Role role)
        {
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(role));
            IRestResponse getResponse = client.Execute(getRequest);
            return JsonConvert.DeserializeObject<List<StudentResponseBody>>(getResponse.Content);
        }
    }
}
