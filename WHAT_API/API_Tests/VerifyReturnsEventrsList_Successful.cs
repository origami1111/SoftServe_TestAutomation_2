using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace WHAT_API
{
    [TestFixture]
    public class VerifyReturnsEventrsList_Successful: BaseTestAPI
    {
        [Test]
        [TestCase(9)]
        public void VerifyReturnsEventrsList_StatusCode200(int mentorID)
        {
            RestClient client = new RestClient(ReaderUrlsJSON.ByName("ApiSchedulesEvent"));
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(new
            {
                mentorID = mentorID
            });
            IRestResponse response = client.Execute(request);
            var credentials = JsonConvert.DeserializeObject<List<EventFilterResponse>>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode");
                foreach (var item in credentials)
                {
                    Assert.AreEqual(mentorID, item.mentorId, "Presence of an item in the list");
                }
            });

        }
    }
}