using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class PUT_UpdateSingleSchedule_Tests : API_BaseTest
    {
        protected RestRequest InitNewRequest(string endPointName, Method method,
            IAuthenticator authenticator)
        {
            var resource = ReaderUrlsJSON.ByName(endPointName, endpointsPath);
            var request = new RestRequest(resource, method);
            authenticator.Authenticate(client, request);
            return request;
        }

        protected T Execute<T>(RestRequest request) where T : new()
        {
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var exception = new Exception(message, response.ErrorException);
                throw exception;
            }
            System.Diagnostics.Debug.WriteLine(response.Content);
            return response.Data;
        }

        [TestCase(Role.Admin)]
        [TestCase(Role.Secretar)]
        public void Test(Role role)
        {
            var authenticator = GetAuthenticatorFor(role);

            var requestData = File.ReadAllText("JsonDataFiles/CreateSchedule.json");
            var expected = JsonConvert.DeserializeObject<CreateSchedule>(requestData);
            
            // POST
            RestRequest postRequest = InitNewRequest("ApiSchedules", Method.POST, authenticator);
            postRequest.AddJsonBody(requestData);
            var originalSchedule = Execute<EventOccurrence>(postRequest);
            
            // PUT
            RestRequest putRequest = InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
                Method.PUT, authenticator);
            putRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            requestData = File.ReadAllText("JsonDataFiles/UpdateSchedule.json");
            putRequest.AddJsonBody(requestData);
            var actualSchedule = Execute<EventOccurrence>(putRequest);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Pattern.Type, actualSchedule.Pattern, "");
                Assert.AreEqual(expected.Context.GroupID, actualSchedule.StudentGroupId);
                CollectionAssert.AreEquivalent(originalSchedule.Events, actualSchedule.Events, "Updated Events");
            });
            
            // DELETE
            RestRequest deleteRequest = InitNewRequest("ApiSchedulesEventOccurrenceID",
                Method.DELETE, authenticator);
            deleteRequest.AddUrlSegment("eventOccurrenceID", originalSchedule.Id.ToString());
            var deleteSchedule = Execute<EventOccurrence>(deleteRequest);
        }
    }
}