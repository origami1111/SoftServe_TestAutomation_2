using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WHAT_Utilities;

namespace WHAT_API
{
    public class ScheduleGenerator : API_BaseTest
    {       
        private CreateSchedule schedule = new CreateSchedule();
        private Account registeredUser;
        Random random = new Random();

        public CreateSchedule GenerateShedule(PatternType type, int interval,
                                              List<DayOfWeek> list, DateTime startDate, DateTime finishDate, 
                                              long? mentorID, long? groupID, long themeID)
        {
            schedule.Pattern = new Pattern()
            {
                Type = type,
                Interval = interval,
                DaysOfWeek = list
            };

            schedule.Range = new OccurrenceRange()
            {
                StartDate = startDate,
                FinishDate = finishDate
            };

            schedule.Context = new Context()
            {
                MentorID = mentorID,
                ThemeID = groupID,
                GroupID = themeID
            };

            return schedule;
        }

        public CreateSchedule GenerateShedule()
        {

            schedule.Pattern = new Pattern()
            {
                Type = PatternType.Daily,
                Interval = random.Next(0, 4)
            };

            schedule.Range = new OccurrenceRange()
            {
                StartDate = DateTime.Now.ToUniversalTime(),
                FinishDate = DateTime.Now.ToUniversalTime()
            };

            schedule.Context = new Context()
            {
                MentorID = GetMentorID(),
                ThemeID = GetThemeID(),
                GroupID = GetStudentsGroupID()
            };
          

            return schedule;
        }

        public int GetMentorID()
        {
            int mentorID;
            registeredUser = RegistrationUser();

            RestClient getClient = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));

            RestRequest  request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsNotAssigned", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            IRestResponse response = client.Execute(request);


            string json = response.Content;
            var users = JsonConvert.DeserializeObject<List<Account>>(json);
            var searchedUser = users.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            registeredUser.Id = searchedUser.Id;

            request = new RestRequest($"mentors/{registeredUser.Id}", Method.POST);
            request.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            response = client.Execute(request);

            request = new RestRequest(ReaderUrlsJSON.ByName("ApiOnlyActiveMentors", endpointsPath), Method.GET);
            request.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            response = client.Execute(request);

            List<Mentor> mentors = JsonConvert.DeserializeObject<List<Mentor>>(response.Content.ToString());
            var searchedMentor = mentors.Where(user => user.Email == registeredUser.Email).FirstOrDefault();
            mentorID = searchedMentor.Id;

            return mentorID;
        }

        public int GetStudentsGroupID()
        {
            int studentsGroupID;
            RestClient getClient = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsGroup", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            IRestResponse getResponse = getClient.Execute(getRequest);
            List<StudentGroup> listOfStudentsGroup = JsonConvert.DeserializeObject<List<StudentGroup>>(getResponse.Content.ToString());
            if (!listOfStudentsGroup.Any() || getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Status code: {getResponse.StatusCode} is not {HttpStatusCode.OK}");
            }
            else
            {
                int randomElement = random.Next(0, listOfStudentsGroup.Count);
                studentsGroupID = listOfStudentsGroup.ElementAt(randomElement).ID;
            }
            return studentsGroupID;
        }

        public int GetThemeID()
        {
            int themeID;
            RestClient getClient = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiThemes", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            IRestResponse getResponse = getClient.Execute(getRequest);
            List<Themes> listOfThemes = JsonConvert.DeserializeObject<List<Themes>>(getResponse.Content.ToString());
            if (!listOfThemes.Any() || getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Status code: {getResponse.StatusCode} is not {HttpStatusCode.OK}");
            }
            else
            {
                int randomElement = random.Next(0, listOfThemes.Count);
                themeID = listOfThemes.ElementAt(randomElement).ID;
            }
            return themeID;
        }
    }
}
