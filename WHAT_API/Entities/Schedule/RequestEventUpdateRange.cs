using Newtonsoft.Json;

namespace WHAT_API.Entity
{
    class RequestEventUpdateRange
    {
        public Filter filter { get; set; }
        public Request request { get; set; }

        public RequestEventUpdateRange WithFilter(Filter filter)
        {
            this.filter = filter;
            return this;
        }

        public RequestEventUpdateRange WithRequest(Request request)
        {
            this.request = request;
            return this;
        }

        public class Filter
        {
            [JsonProperty("courseID")]
            public object CourseId { get; set; }
            [JsonProperty("mentorID")]
            public int MentorId { get; set; }
            [JsonProperty("groupID")]
            public object GroupId { get; set; }
            [JsonProperty("themeID")]
            public object ThemeId { get; set; }
            [JsonProperty("studentAccountID")]
            public object StudentAccountId { get; set; }
            [JsonProperty("eventOccurrenceID")]
            public object EventOccurrenceId { get; set; }
            [JsonProperty("startDate")]
            public string StartDate { get; set; }
            [JsonProperty("finishDate")]
            public string FinishDate { get; set; }
            public Filter WithCourseId(object CourseId)
            {
                this.CourseId = CourseId;
                return this;
            }

            public Filter WithMentorId(int mentorId)
            {
                MentorId = mentorId;
                return this;
            }

            public Filter WithGroupId(object groupId)
            {
                GroupId = groupId;
                return this;
            }

            public Filter WithThemeId(object themeID)
            {
                ThemeId = themeID;
                return this;
            }

            public Filter WithSudentAccountId(object studentAccountId)
            {
                StudentAccountId = studentAccountId;
                return this;
            }

            public Filter WithEventOccurenceId(object eventOccurrenceId)
            {
                EventOccurrenceId = eventOccurrenceId;
                return this;
            }

            public Filter WithStartDate(string startDate)
            {
                StartDate = startDate;
                return this;
            }

            public Filter WithFinishDate(string finishDate)
            {
                FinishDate = finishDate;
                return this;
            }
        }

        public class Request
        {
            [JsonProperty("studentGroupId")]
            public int StudentGroupId { get; set; }
            [JsonProperty("themeId")]
            public int ThemeId { get; set; }
            [JsonProperty("mentorId")]
            public int MentorId { get; set; }
            [JsonProperty("eventStart")]
            public string EventStart { get; set; }
            [JsonProperty("eventEnd")]
            public string EventEnd { get; set; }

            public Request WithStudentGroupId(int studentGroupId)
            {
                StudentGroupId = studentGroupId;
                return this;
            }

            public Request WithThemeId(int themeId)
            {
                ThemeId = themeId;
                return this;
            }

            public Request WithMentorId(int mentorId)
            {
                MentorId = mentorId;
                return this;
            }

            public Request WithEventStart(string eventStart)
            {
                EventStart = eventStart;
                return this;
            }

            public Request WithEventEnd(string eventEnd)
            {
                EventEnd = eventEnd;
                return this;
            }
        }
    }
}
