using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WHAT_API
{
    public class Pattern
    {
        [JsonProperty("type")] // Required
        public PatternType Type { get; set; }

        [JsonProperty("interval")] // Required
        public int Interval { get; set; }

        [JsonProperty("daysOfWeek")]
        public IList<DayOfWeek> DaysOfWeek { get; set; }

        [JsonProperty("index")]
        public MonthIndex? Index { get; set; }

        [JsonProperty("dates")] // Range(1, 31)
        public IList<int> Dates { get; set; }
    }
}
