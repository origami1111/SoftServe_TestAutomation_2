using Newtonsoft.Json;
using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_API.Entities;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    [TestFixture]
    [AllureNUnit]
    public class GetLessonInformationByLessonId : API_BaseTest
    {
        [TestCaseSource(typeof(TestCase), nameof(TestCase.ValidRole))]
        public void GetLessonInformationById(HttpStatusCode expectedStatusCode,Role role)
        {
            api.log = LogManager.GetLogger($"Lessons/{nameof(GetLessonInformationByLessonId)}");
            var request = api.InitNewRequest("ApiStudentsGroup", Method.GET, api.GetAuthenticatorFor(Role.Admin));
            var response = APIClient.client.Execute(request);
            var responseDetail = JsonConvert.DeserializeObject<List<Lesson>>(response.Content);
            int id = responseDetail
                .Select(l=>l.Id)
                .FirstOrDefault();
            
            var newRequest = new RestRequest($"lessons/{id}", Method.GET)
                .AddHeader("Authorization", api.GetToken(role));
            var newResponse = APIClient.client.Execute(newRequest);
            var actualCode = newResponse.StatusCode;
            api.log.Info($"Request is done with {actualCode} StatusCode");
            Assert.AreEqual(expectedStatusCode, actualCode, "Status Code Assert");
            var resposneDetaile = JsonConvert.DeserializeObject<Lesson>(newResponse.Content);
            Assert.AreEqual(resposneDetaile.Id, id,"Assert  lesson id");
            api.log.Info($"Expected and actual results is checked");
        }
    }
}
