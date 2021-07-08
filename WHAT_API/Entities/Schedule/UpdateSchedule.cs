using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class UpdateSchedule
    {
        [JsonProperty("lessonStart")] // Required
        public TimeSpan LessonStart { get; set; }

        [JsonProperty("lessonEnd")] // Required
        public TimeSpan LessonEnd { get; set; }

        [JsonProperty("repeatRate")] // Required
        public PatternType RepeatRate { get; set; }

        [JsonProperty("dayNumber")] // Range(1, 31)
        public uint? DayNumber { get; set; }
    }
}

