using Newtonsoft.Json;
using System.Collections.Generic;

namespace WHAT_Utilities
{
    public class StudentGroupDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("courseId")]
        public int CourseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("finishDate")]
        public string FinishDate { get; set; }

        [JsonProperty("studentIds")]
        public List<int> StudentIds { get; set; }

        [JsonProperty("mentorIds")]
        public List<int> MentorIds { get; set; }
    }
}
