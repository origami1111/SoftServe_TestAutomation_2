using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WHAT_API
{
    public class StudentsGroup
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("courseId")]
        public int CourseId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("finishDate")]
        public DateTime FinishDate { get; set; }
        [JsonProperty("studentIds ")]
        public List<int> StudentIds { get; set; }
        [JsonProperty("mentorIds")]
        public List<int> MentorIds { get; set; }
    }
}
