using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class ScheduledEventFilterRequest
    {
        [JsonProperty("courseID")]
        public long? CourseID { get; set; }

        [JsonProperty("mentorID")]
        public long? MentorID { get; set; }

        [JsonProperty("groupID")]
        public long? GroupID { get; set; }

        [JsonProperty("themeID")]
        public long? ThemeID { get; set; }

        [JsonProperty("studentAccountID")]
        public long? StudentAccountID { get; set; }

        [JsonProperty("eventOccurrenceID")]
        public long? EventOccurrenceID { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("finishDate")]
        public DateTime? FinishDate { get; set; }
    }
}
