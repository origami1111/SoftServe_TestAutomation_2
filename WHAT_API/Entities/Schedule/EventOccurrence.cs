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
            EventOccurrence other = (EventOccurrence)obj;

            return (this.Id == other.Id
                && this.StudentGroupId == other.StudentGroupId
                && this.EventStart == other.EventStart
                && this.EventFinish == other.EventFinish
                && this.Pattern == other.Pattern
                && SequencesEqual(this.Events, other.Events)
                && this.Storage == other.Storage);
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

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

    }
}
