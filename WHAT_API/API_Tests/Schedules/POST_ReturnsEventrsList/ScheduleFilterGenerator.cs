using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using WHAT_Utilities;

namespace WHAT_API
{
    public class ScheduleFilterGenerator:API_BaseTest
    {
        private ScheduleGenerator generator = new ScheduleGenerator();

        public LessonsForMentor GetProperFilter(Role role)
        {
            LessonsForMentor item = new LessonsForMentor();
            bool isValidId = false;
            while (!isValidId)
            {
                int mentorID = generator.GetMentorID();
                //RestRequest request = new RestRequest($"mentors/{mentorID}/lessons", Method.GET);
                RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiMentorsIdLessons", endpointsPath), Method.GET);
                request = InitNewRequest("ApiMentorsIdLessons", Method.GET, GetAuthenticatorFor(role));
                request.AddUrlSegment("id", mentorID.ToString());
                //request.AddHeader("Authorization", GetToken(role));
                request.AddParameter("id", mentorID);
                IRestResponse response = client.Execute(request);
                var listLessonsForMentors = JsonConvert.DeserializeObject<List<LessonsForMentor>>(response.Content);
                if (listLessonsForMentors.Any())
                {
                    isValidId = true;
                    item = listLessonsForMentors.First();
                }
            }
            return item;
        }

        public StudentsGroup GetStudentsGroup(int studentGroupId, Role role)
        {
            RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsGroupId", endpointsPath), Method.GET);
            request = InitNewRequest("ApiStudentsGroupId", Method.GET, GetAuthenticatorFor(role));
            request.AddUrlSegment("id", studentGroupId.ToString());
            request.AddParameter("id", studentGroupId);
            IRestResponse response = client.Execute(request);
            var listLessonsForMentors = JsonConvert.DeserializeObject<StudentsGroup>(response.Content);
            return listLessonsForMentors;
        }
    }
}
