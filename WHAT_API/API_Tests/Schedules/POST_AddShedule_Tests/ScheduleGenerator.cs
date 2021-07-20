using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    public class ScheduleGenerator : API_BaseTest
    {
        private CreateSchedule schedule = new CreateSchedule();
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
                Interval = random.Next(1, 4)
            };

            schedule.Range = new OccurrenceRange()
            {
                StartDate = new DateTime(2019, 1, 2, 13, 27, 09).ToUniversalTime(),
                FinishDate = new DateTime(2021, 7, 7, 15, 27, 09).ToUniversalTime()
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
            RestClient getClient = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiOnlyActiveMentors", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            IRestResponse getResponse = getClient.Execute(getRequest);
            List<Mentors> listOfMentors = JsonConvert.DeserializeObject<List<Mentors>>(getResponse.Content.ToString());
            if (!listOfMentors.Any() || getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
            else
            {
                int randomElement = random.Next(0, listOfMentors.Count);
                mentorID = listOfMentors.ElementAt(randomElement).ID;
            }
            return mentorID;
        }

        public int GetStudentsGroupID()
        {
            int studentsGroupID;
            RestClient getClient = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
            RestRequest getRequest = new RestRequest(ReaderUrlsJSON.ByName("ApiStudentsGroup", endpointsPath), Method.GET);
            getRequest.AddHeader("Authorization", GetToken(Role.Admin, getClient));
            IRestResponse getResponse = getClient.Execute(getRequest);
            List<StudentsGroup> listOfStudentsGroup = JsonConvert.DeserializeObject<List<StudentsGroup>>(getResponse.Content.ToString());
            if (!listOfStudentsGroup.Any() || getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
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
                throw new Exception();
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
