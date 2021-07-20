using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.IO;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class CoursesTests : API_BaseTest
    {
        [TestCase(Role.Admin)]
        [TestCase(Role.Secretary)]
        public void PUT_UpdateSingleSchedule(Role role)
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