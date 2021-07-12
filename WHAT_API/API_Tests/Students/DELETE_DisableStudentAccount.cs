using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [TestFixture]
    public class DELETE_DisableStudentAccount:API_BaseTest
    {
        [Test]
        public void VerifyDeletingStudentAccount_Valid([Values(Role.Admin, Role.Secretar, Role.Mentor)] Role user)
        {
            int maxId = GetActiveStudentsList(user).Max(s => s.Id);
            log.Info($"List of students is taken, there are {GetActiveStudentsList(user).Count} active students");
            DeleteStudentsAccount(maxId);
            log.Info($"Deleted students with max id : {maxId}");
            int expect = --maxId;
            int actual = GetActiveStudentsList(user).Max(s => s.Id);
            log.Info($"List of students is taken, there are {GetActiveStudentsList(user).Count} active students");
            Assert.AreEqual(expect, actual);
        }
        private List<Student> GetActiveStudentsList(Role role)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(role));
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<Student>>(response.Content);
        }
        private void DeleteStudentsAccount(int maxId)
        {
            RestRequest request = new RestRequest($"students/{maxId}", Method.DELETE);
            request.AddHeader("Authorization", GetToken(Role.Admin));
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }
    }
}
