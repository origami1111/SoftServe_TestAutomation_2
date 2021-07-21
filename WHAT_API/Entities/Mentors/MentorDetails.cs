using Newtonsoft.Json;

namespace WHAT_API.Entities.Lessons
{
    public class MentorDetails
    {
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("account")]
        public object Account { get; set; }
        [JsonProperty("lesson")]
        public object[] Lesson { get; set; }
        [JsonProperty("mentorsOfCourses")]
        public object[] MentorsOfCourses { get; set; }
        [JsonProperty("mentorsOfStudentGroups")]
        public object[] MentorsOfStudentGroups { get; set; }
        [JsonProperty("scheduledEvents")]
        public object ScheduledEvents { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
