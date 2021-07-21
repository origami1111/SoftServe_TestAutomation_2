using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WHAT_API
{
    public class ScheduledEvent : IEquatable<ScheduledEvent>
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
            return this.Equals(ev);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public bool Equals([AllowNull] ScheduledEvent that)
        {
            return (that != null
                && this.EventOccuranceId == that.EventOccuranceId
                && this.StudentGroupId == that.StudentGroupId
                && this.ThemeId == that.ThemeId
                && this.MentorId == that.MentorId
                && this.LessonId == that.LessonId
                && this.EventStart == that.EventStart
                && this.EventFinish == that.EventFinish);
        }
    }
}
