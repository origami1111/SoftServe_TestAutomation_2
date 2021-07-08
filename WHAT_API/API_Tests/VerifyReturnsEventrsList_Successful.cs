using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class VerifyReturnsEventrsList_Successful: API_BaseTest
    {
        [Test]
        [TestCase(9)]
        [TestCase(8)]

        public void VerifyReturnsEventrsList_StatusCode200(int mentorID)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            request.AddHeader("Authorization", GetToken("admin.@gmail.com", "admiN_12"));
            request.AddJsonBody(new
            {
                mentorID = mentorID
            });
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
            var events = JsonConvert.DeserializeObject<List<EventFilterResponse>>(response.Content);
            Assert.Multiple(() =>
            {
                foreach (var item in events)
                {
                    Assert.AreEqual(mentorID, item.MentorId, "Presence of an item in the list");
                }
            });

        }
    }
}