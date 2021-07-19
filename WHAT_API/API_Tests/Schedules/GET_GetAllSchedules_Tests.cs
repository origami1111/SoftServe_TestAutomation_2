using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    class GET_GetAllSchedules_Tests : API_BaseTest
    {
        //[TestCase(Role.Admin)]
        //[TestCase(Role.Secretary)]
        //public void Test(Role role)
        //{
        //    var authenticator = GetAuthenticatorFor(role);
        //    var requestData = File.ReadAllText("JsonDataFiles/CreateSchedule.json");
        //    var expectedOriginalOccurence = JsonConvert.DeserializeObject<CreateSchedule>(requestData);

        //    // POST
        //    RestRequest postRequest = InitNewRequest("ApiSchedules", Method.POST, authenticator);
        //    postRequest.AddJsonBody(requestData);
        //    var originalOccurence = Execute<EventOccurrence>(postRequest);

        //    //GET
        //    RestRequest getRequest = InitNewRequest("ApiSchedulesEventOccurrences", Method.GET, authenticator);
        //    IRestResponse<List<EventOccurrence>> occurences = client.Execute<List<EventOccurrence>>(getRequest);
        //    var actualOriginalOccurence = occurences.Data.Last();

        //    // PUT
        //    RestRequest putRequest = InitNewRequest("ApiSchedulesEventOccurrences-eventOccurrenceID",
        //        Method.PUT, authenticator);
        //    putRequest.AddUrlSegment("eventOccurrenceID", originalOccurence.Id.ToString());
        //    requestData = File.ReadAllText("JsonDataFiles/UpdateSchedule.json");
        //    putRequest.AddJsonBody(requestData);
        //    Execute<EventOccurrence>(putRequest);
        //    var expectedUpdatedOccurence = JsonConvert.DeserializeObject<CreateSchedule>(requestData);

        //    //GET
        //    occurences = client.Execute<List<EventOccurrence>>(getRequest);/////
        //    EventOccurrence actualUpdatedOccurence = occurences.Data.Last();

        //    Assert.AreEqual(expectedOriginalOccurence, actualOriginalOccurence);


        //    // DELETE
        //    RestRequest deleteRequest = InitNewRequest("ApiSchedulesEventOccurrenceID",
        //        Method.DELETE, authenticator);
        //    deleteRequest.AddUrlSegment("eventOccurrenceID", originalOccurence.Id.ToString());
        //    Execute<EventOccurrence>(deleteRequest);

        //}
    }
}
