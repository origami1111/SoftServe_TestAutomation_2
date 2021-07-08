using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class VerifyReturnsEventrsList_BadRequest : API_BaseTest
    {
        [Test]
        [TestCase(12050125, null, 1, 1, 1, 1, "2020-10-12T10:15:00", "2020-10-12T10:15:00")]
        [TestCase(null, null, null, null, null, null, "2021-10-12T10:15:00", "2021-10-12T10:15:00")]
        public void VerifyReturnsEventrsList_StatusCode400(int courseID,
            int mentorID, int groupID, int themeID, int studentAccountID, int eventOccurrenceID,
            DateTime startDate, DateTime finishDate)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            request.AddHeader("Authorization", GetToken("admin.@gmail.com", "admiN_12"));
            request.AddJsonBody(new
            {
                courseID = courseID,
                mentorID = mentorID,
                groupID = groupID,
                themeID = themeID,
                studentAccountID = studentAccountID,
                eventOccurrenceID = eventOccurrenceID,
                startDate = startDate,
                finishDate = finishDate
            });
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
