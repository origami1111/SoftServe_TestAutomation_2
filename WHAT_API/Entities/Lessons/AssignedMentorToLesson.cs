using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WHAT_API.Entities.Lessons
{
    public class AssignedMentorToLesson
    {
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("studentGroupId")]
        public int StudentGroupId { get; set; }
        [JsonProperty("themeId")]
        public int ThemeId { get; set; }
        [JsonProperty("lessonDate")]
        public DateTime LessonDate { get; set; }
        [JsonProperty("mentor")]
        public MentorDetails Mentor { get; set; }
        [JsonProperty("studentGroup")]
        public object StudentGroup { get; set; }
        [JsonProperty("theme")]
        public object Theme { get; set; }
        [JsonProperty("homeworks")]
        public object Homeworks { get; set; }
        [JsonProperty("visits")]
        public List<Visit> Visits { get; set; }
        [JsonProperty("scheduledEvent")]
        public object ScheduledEvent { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
