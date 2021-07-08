using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class OccurrenceRange
    {
        [JsonProperty("startDate")] // Required
        public DateTime StartDate { get; set; }

        [JsonProperty("finishDate")]
        public DateTime? FinishDate { get; set; }
    }
}
