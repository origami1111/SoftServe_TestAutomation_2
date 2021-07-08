using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WHAT_API
{
    public class EventOccurrence
    {
        [JsonProperty("id")] // Required
        public long? Id { get; set; }

        [JsonProperty("studentGroupId")] // Required
        public long StudentGroupId { get; set; }

        [JsonProperty("eventStart")] // Required
        public DateTime EventStart { get; set; }

        [JsonProperty("eventFinish")] // Required
        public DateTime EventFinish { get; set; }

        [JsonProperty("pattern")] // Required
        public PatternType Pattern { get; set; }

        [JsonProperty("events")]
        public IList<ScheduledEvent> Events { get; set; }

        [JsonProperty("storage")]
        public long Storage { get; set; }
    }
}
