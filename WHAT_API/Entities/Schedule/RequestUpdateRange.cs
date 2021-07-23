using Newtonsoft.Json;
using System;

namespace WHAT_API.Entity
{
    public class RequestUpdateRange
    {
        [JsonProperty("studentGroupId")]
        public int StudentGroupId { get; set; }
        [JsonProperty("themeId")]
        public int ThemeId { get; set; }
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("eventStart")]
        public DateTime EventStart { get; set; }
        [JsonProperty("eventEnd")]
        public DateTime EventEnd { get; set; }

        public RequestUpdateRange WithStudentGroupId(int studentGroupId)
        {
            StudentGroupId = studentGroupId;
            return this;
        }

        public RequestUpdateRange WithThemeId(int themeId)
        {
            ThemeId = themeId;
            return this;
        }

        public RequestUpdateRange WithMentorId(int mentorId)
        {
            MentorId = mentorId;
            return this;
        }

        public RequestUpdateRange WithEventStart(DateTime eventStart)
        {
            EventStart = eventStart;
            return this;
        }

        public RequestUpdateRange WithEventEnd(DateTime eventEnd)
        {
            EventEnd = eventEnd;
            return this;
        }
    }
}
