using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class UpdateScheduledEvent
    {
        [JsonProperty("studentGroupId")]
        public long? StudentGroupId { get; set; }

        [JsonProperty("themeId")]
        public long? ThemeId { get; set; }

        [JsonProperty("mentorId")]
        public long? MentorId { get; set; }

        [JsonProperty("eventStart")]
        public DateTime? EventStart { get; set; }

        [JsonProperty("eventEnd")]
        public DateTime? EventEnd { get; set; }
    }
}

