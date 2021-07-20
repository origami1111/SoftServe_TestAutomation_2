using NLog;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.POST_ReturnsEventrsList
{
    [TestFixture]
    public class POST_ReturnsEventrsList_BadRequest : API_BaseTest
    {
        public POST_ReturnsEventrsList_BadRequest()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventrsList_BadRequest)}");
        }

        private static IEnumerable<TestCaseData> FilterRulesSources
        {
            get
            {
                yield return new TestCaseData(12050125, null, 1, 1, 1, 1, null, null);
                yield return new TestCaseData(12050125, null, 1, 1, 1, 1, new DateTime(2019,7,7,15,27,09).ToUniversalTime(), null);
                yield return new TestCaseData(12050125, null, null, null, null, null, null, new DateTime(2019, 7, 7, 15, 27, 09).ToUniversalTime());
            }
        }

        [Test]
        [TestCaseSource(nameof(FilterRulesSources))]
        public void VerifyReturnsEventrsList_StatusCode400(int courseID,
            int mentorID, int groupID, int themeID, int studentAccountID, int eventOccurrenceID,
            DateTime startDate, DateTime finishDate)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
            log.Info($"POST request to {ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath)}");
            request.AddHeader("Authorization", GetToken(Role.Admin));
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
            log.Info($"Request is done with {response.StatusCode} StatusCode");
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            log.Info($"Expected and actual results is checked");
        }
    }
}
