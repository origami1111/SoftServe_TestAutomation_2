using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [AllureNUnit]
    [TestFixture]
    public class PUT_UpdatesStudent: API_BaseTest
    {
        private RestRequest request;
        private IRestResponse response;
        public PUT_UpdatesStudent()
        {
            log = LogManager.GetLogger($"Students/{nameof(PUT_UpdatesStudent)}");
        }
        /// <summary>
        /// That auto test verify updating students using PUT request / Student section with Admin/Secretary role
        /// 
        /// Steps:
        /// 1.Generate student information for updating
        /// 2.Update student information using PUT request / Student section
        /// 3.Сheck that the list has decreased by one
        /// </summary>
        [Test]
        [TestCase( Role.Admin)]
        [TestCase( Role.Secretary)]
        public void VerifyDeletingStudentAccount_Valid(Role role)
        {
            StudentDetails randomStudent = GetRandomActiveStudent(role);
            log.Info($"Student information is generated: id={randomStudent.Id}");
            request = InitNewRequest("ApiStudentsStudentId", Method.PUT, GetAuthenticatorFor(role));
            request.AddUrlSegment("studentId", randomStudent.Id.ToString());
            request.AddParameter("studentId", randomStudent.Id);
            UpdateStudent updateRequestBody = new UpdateStudent();
            updateRequestBody.FirstName = randomStudent.FirstName;
            updateRequestBody.LastName = randomStudent.LastName;
            updateRequestBody.Email = randomStudent.Email;
            var updateStudent = JsonConvert.SerializeObject(updateRequestBody);
            request.AddJsonBody(updateStudent);
            log.Info($"PUT request to {ReaderUrlsJSON.ByName("ApiStudentsStudentId", endpointsPath)}");
            response = client.Execute(request);

            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var changedStudent = JsonConvert.DeserializeObject<UpdateStudent>(response.Content);
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
            log.Info($"GET request to {ReaderUrlsJSON.ByName("ApiStudentsStudentId", endpointsPath)}");
            response = client.Execute(request);
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var student = JsonConvert.DeserializeObject<StudentDetails>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomStudent.Email, student.Email);
                Assert.AreEqual(randomStudent.FirstName, student.FirstName);
                Assert.AreEqual(randomStudent.LastName, student.LastName);
                log.Info($"Expected and actual results is checked");
            });
        }

        /// <summary>
        /// Get and verify random active student from student lists
        /// </summary>
        /// <returns>
        /// StudentResponseBody entity
        /// </returns>
        private StudentDetails GetRandomActiveStudent(Role role)
        {
            const int MIN_RANDOM = 4;
            Random random = new Random();
            StudentDetails randomStudent = new StudentDetails();
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsActive", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(role));
            IRestResponse getResponse = client.Execute(getRequest);
            var listOfActiveStudentsJsonConvert = JsonConvert.DeserializeObject<List<StudentDetails>>(getResponse.Content);
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
