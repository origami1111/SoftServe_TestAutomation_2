using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_API.Entity;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    [AllureNUnit]
    public class PUT_EventUpdateRange : API_BaseTest
    {
        [TestCase(HttpStatusCode.OK, Role.Admin, 1)]
        [TestCase(HttpStatusCode.OK, Role.Secretary, 1)]
        public void MethodPutEventUpdateRange(HttpStatusCode expectedStatusCode, Role role, int themeId)
        {
            log = LogManager.GetLogger($"Schedules/{nameof(PUT_EventUpdateRange)}");
            var StudentGroupRequest = InitNewRequest("ApiStudentsGroup", Method.GET, GetAuthenticatorFor(Role.Admin));
            var StudentGroupResponse = client.Execute(StudentGroupRequest);
            var responseDetail = JsonConvert.DeserializeObject<List<StudentGroup>>(StudentGroupResponse.Content);
            var group = responseDetail.First();
            int mentorId = group.MentorIds.First();
            FilterUpdateRange filter = new FilterUpdateRange()
                .WithMentorId(mentorId)
                .WithStartDate(group.StartDate)
                .WithFinishDate(group.FinishDate);
            RequestUpdateRange myRequest = new RequestUpdateRange()
                .WithStudentGroupId(group.Id)
                .WithThemeId(themeId)
                .WithMentorId(mentorId)
                .WithEventStart(group.StartDate)
                .WithEventEnd(group.FinishDate);
            RequestEventUpdateRange mainRequest = new RequestEventUpdateRange()
                .WithFilter(filter)
                .WithRequest(myRequest);
            var jsonfile = JsonConvert.SerializeObject(mainRequest);
            
            var request = InitNewRequest("ApiSchedulesEventsUpdateRange", Method.PUT, GetAuthenticatorFor(role)).AddJsonBody(jsonfile);
            var response = client.Execute(request);
            var actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<List<EventFilterResponse>>(response.Content);
            Assert.Multiple(() =>
            {
                foreach (var item in resposneDetaile)
                {
                    Assert.AreEqual(mentorId, item.MentorId, "Mentor Id Assert");
                    Assert.AreEqual(group.Id, item.StudentGroupId, "Student Group Id Assert");
                    Assert.AreEqual(themeId, item.ThemeId, "Thema Id Assert");
                    Assert.LessOrEqual(group.StartDate, item.EventStart, "Start Date Assert");
                    Assert.LessOrEqual(item.EventFinish, group.FinishDate, "Finish Date Assert");
                }
            });
            log.Info($"Expected and actual results is checked");
        }

        [TestCase(HttpStatusCode.Forbidden, Role.Mentor)]
        [TestCase(HttpStatusCode.Forbidden, Role.Student)]
        public void VerifyForbiddenStatusCode(HttpStatusCode expectedStatusCode, Role role)
        {
            log = LogManager.GetLogger($"Schedules/{nameof(PUT_EventUpdateRange)}");
            var request = InitNewRequest("ApiSchedulesEventsUpdateRange", Method.PUT, GetAuthenticatorFor(role));
            var response = client.Execute(request);
            var actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            log.Info($"Expected and actual results is checked");
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        public void VerifyUnauthorizedStatusCode(HttpStatusCode expectedStatusCode)
        {
            log = LogManager.GetLogger($"Schedules/{nameof(PUT_EventUpdateRange)}");
            var request = new RestRequest(ReaderUrlsJSON.GetUrlByName("ApiSchedulesEventsUpdateRange", endpointsPath), Method.PUT);
            var response = client.Execute(request);
            var actualStatusCode = response.StatusCode;
            log.Info($"Request is done with {actualStatusCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "Status Code Assert");
            log.Info($"Expected and actual results is checked");
        }
    }
}
