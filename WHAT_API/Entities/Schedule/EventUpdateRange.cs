using Newtonsoft.Json;

namespace WHAT_API
{
    public class EventUpdateRange
    {
        [JsonProperty("filter")] // Required
        public ScheduledEventFilterRequest Filter { get; set; }

        [JsonProperty("request")] // Required
        public UpdateScheduledEvent Request { get; set; }

    }
}
