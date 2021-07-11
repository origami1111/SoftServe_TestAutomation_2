using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public override bool Equals(object obj)
        {
            EventOccurrence sc = (EventOccurrence)obj;

            return (this.Id == sc.Id
                && this.StudentGroupId == sc.StudentGroupId
                && this.EventStart == sc.EventStart
                && this.EventFinish == sc.EventFinish
                && this.Pattern == sc.Pattern
                && SequencesEqual(this.Events, sc.Events)
                && this.Storage == sc.Storage);
        }

        private bool SequencesEqual(IEnumerable<ScheduledEvent> a, IEnumerable<ScheduledEvent> b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }

            return a.SequenceEqual(b);
        }

    }
}
