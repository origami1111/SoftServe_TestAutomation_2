using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WHAT_API.Entity;
using WHAT_Utilities;
using static WHAT_API.Entity.RequestEventUpdateRange;

namespace WHAT_API
{
    [TestFixture]
    public class SchedulesPutEventUpdateRange : API_BaseTest
    {
        [Test]
        [TestCase(30, "2020-07-08T09:02:31", "2021-07-08T10:02:31", 1, 1)]
        public void MethodPutEventUpdateRange(int mentorId, string startDate, string finishDate, int studentGroupId, int themeId)
        {

            Filter filter = new Filter().WithMentorId(mentorId).WithStartDate(startDate).WithFinishDate(finishDate);
            Request myRequest = new Request().WithStudentGroupId(studentGroupId).WithThemeId(themeId).WithMentorId(mentorId).WithEventStart(startDate).WithEventEnd(finishDate);
            RequestEventUpdateRange mainRequest = new RequestEventUpdateRange().WithFilter(filter).WithRequest(myRequest);
            var jsonfile = JsonConvert.SerializeObject(mainRequest);

            var request = new RestRequest("schedules/events/updateRange", Method.PUT)
                .AddHeader("Authorization", GetToken(Role.Admin))
                .AddJsonBody(jsonfile);

            var response = client.Execute(request);
            var expectedCode = HttpStatusCode.OK;
            var actualCode = response.StatusCode;
            Assert.AreEqual(expectedCode, actualCode, "Status Code Assert");

            var resposneDetaile = JsonConvert.DeserializeObject<List<EventFilterResponse>>(response.Content);
            var expectedStartData = Convert.ToDateTime(startDate);
            var expectedFinishData = Convert.ToDateTime(finishDate);
            Assert.Multiple(() =>
            {
                foreach (var item in resposneDetaile)
                {
                    Assert.AreEqual(mentorId, item.MentorId, "Mentor Id Assert");
                    Assert.AreEqual(studentGroupId, item.StudentGroupId, "Student Group Id Assert");
                    Assert.AreEqual(themeId, item.ThemeId, "Thema Id Assert");
                    Assert.LessOrEqual(expectedStartData, item.EventStart, "Start Date Assert");
                    Assert.LessOrEqual(item.EventFinish, expectedFinishData, "Finish Date Assert");
                }
            });
        }
    }
}
