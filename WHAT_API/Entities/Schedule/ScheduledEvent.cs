using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class ScheduledEvent
    {
        [JsonProperty("id")] // Required
        public long Id { get; set; }
        
        [JsonProperty("eventOccuranceId")] // Required
        public long EventOccuranceId { get; set; }
        
        [JsonProperty("studentGroupId")] // Required
        public long StudentGroupId { get; set; }

        [JsonProperty("themeId")]
        public long? ThemeId { get; set; }

        [JsonProperty("mentorId")]
        public long? MentorId { get; set; }

        [JsonProperty("lessonId")]
        public long? LessonId { get; set; }
       
        [JsonProperty("eventStart")] // Required
        public DateTime EventStart { get; set; }
        
        [JsonProperty("eventFinish")] // Required
        public DateTime EventFinish { get; set; }
    }
}
