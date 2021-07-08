using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.IO;
using System.Net;

namespace WHAT_API
{
    [TestFixture]
    public class PUT_UpdateSingleSchedule_Tests : API_BaseTest
    {
        private IRestResponse response;
        private string requestData;

        [OneTimeSetUp]
        public void GetResponse()
        {
            var adminToken = GetToken("admin.@gmail.com", "admiN_12");
            var secretaryToken = GetToken("secretary@gmail.com", "What_123");

            string endPoint = $"schedules/eventOccurrences/{1}";
            var request = new RestRequest(endPoint, Method.PUT);
            request.AddHeader("Authorization", adminToken?.ToString());
            request.AddHeader("Accept", "application/json, text/plain, */*");

            requestData = File.ReadAllText("JsonDataFiles/CreateSchedule.json");
            request.AddJsonBody(requestData);

            response = client.Execute(request);
        }

        [Test]
        public void StatusCodeTest()
        {
            var expect = HttpStatusCode.OK;
            var actual = response.StatusCode;
            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void Test()
        {
            var expected = JsonConvert.DeserializeObject<CreateSchedule>(requestData);

            System.Diagnostics.Debug.WriteLine(response.Content);
            var actual = JsonConvert.DeserializeObject<EventOccurrence>(response.Content);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Pattern.Type, actual.Pattern);
                Assert.AreEqual(expected.Context.GroupID, actual.StudentGroupId);
            });
        }
    }
}