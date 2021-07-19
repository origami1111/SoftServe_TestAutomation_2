using Newtonsoft.Json;
using NLog;
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
    public class PUT_UpdatesStudent: API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        public PUT_UpdatesStudent()
        {
            log = LogManager.GetLogger($"Students/{nameof(PUT_UpdatesStudent)}");
        }

        [Test]
        [TestCase( Role.Admin)]
        [TestCase( Role.Secretary)]
        public void VerifyDeletingStudentAccount_Valid(Role role)
        {
            StudentResponseBody randomStudent = GetRandomActiveStudent(role);
            request = InitNewRequest("ApiStudentsStudentId", Method.PUT, GetAuthenticatorFor(role));
            request.AddUrlSegment("studentId", randomStudent.Id.ToString());
            request.AddParameter("studentId", randomStudent.Id);
            StudentUpdateRequestBody updateRequestBody = new StudentUpdateRequestBody();
            updateRequestBody.FirstName = randomStudent.FirstName;
            updateRequestBody.LastName = randomStudent.LastName;
            updateRequestBody.Email = randomStudent.Email;
            var updateStudent = JsonConvert.SerializeObject(updateRequestBody);
            request.AddJsonBody(updateStudent);
            response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var changedStudent = JsonConvert.DeserializeObject<StudentRequestBody>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomStudent.Email, changedStudent.Email);
                Assert.AreEqual(randomStudent.FirstName, changedStudent.FirstName);
                Assert.AreEqual(randomStudent.LastName, changedStudent.LastName);
                log.Info($"Expected and actual results is checked");
            });
            request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsId", endpointsPath), Method.GET);
            request = InitNewRequest("ApiStudentsId", Method.GET, GetAuthenticatorFor(role));
            request.AddUrlSegment("id", randomStudent.Id.ToString());
            request.AddParameter("id", randomStudent.Id);
            response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //
            var student = JsonConvert.DeserializeObject<StudentResponseBody>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomStudent.Email, student.Email);
                Assert.AreEqual(randomStudent.FirstName, student.FirstName);
                Assert.AreEqual(randomStudent.LastName, student.LastName);
                log.Info($"Expected and actual results is checked");
            });
        }

        private StudentResponseBody GetRandomActiveStudent(Role role)
        {
            const int MIN_RANDOM = 4;
            Random random = new Random();
            StudentResponseBody randomStudent = new StudentResponseBody();
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(role));
            IRestResponse getResponse = client.Execute(getRequest);
            var listOfActiveStudentsJsonConvert = JsonConvert.DeserializeObject<List<StudentResponseBody>>(getResponse.Content);
            if (!listOfActiveStudentsJsonConvert.Any() || getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
            else
            {
                int randomElement = random.Next(MIN_RANDOM, listOfActiveStudentsJsonConvert.Count);
                randomStudent = listOfActiveStudentsJsonConvert.ElementAt(randomElement);
            }
            return randomStudent;
        }

        
    }
}
