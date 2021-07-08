using Newtonsoft.Json;

namespace WHAT_API
{
    public class CreateSchedule
    {
        [JsonProperty("pattern")] // Required
        public Pattern Pattern { get; set; }

        [JsonProperty("range")] // Required
        public OccurrenceRange Range { get; set; }

        [JsonProperty("context")] // Required
        public Context Context { get; set; }
    }
}
