using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WHAT_API.Entities
{
    public class Lesson
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("studentGroupId")]
        public int StudentGroupId { get; set; }
        [JsonProperty("lessonDate")]
        public DateTime LessonDate { get; set; }
        [JsonProperty("lessonVisits")]
        public List<LessonVisit> LessonVisits { get; set; }
    }
}
