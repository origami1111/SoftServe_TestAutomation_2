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

        public override bool Equals(object obj)
        {
            ScheduledEvent ev = (ScheduledEvent)obj;

            return (this.Id == ev.Id
                && this.EventOccuranceId == ev.EventOccuranceId
                && this.StudentGroupId == ev.StudentGroupId
                && this.ThemeId == ev.ThemeId
                && this.MentorId == ev.MentorId
                && this.LessonId == ev.LessonId
                && this.EventStart == ev.EventStart
                && this.EventFinish == ev.EventFinish);
        }

    }
}
