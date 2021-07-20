using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class LessonsForMentor
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
        public Lessonvisit[] LessonVisits { get; set; }
        public class Lessonvisit
        {
            [JsonProperty("studentId")]
            public int StudentId { get; set; }
            [JsonProperty("studentMark")]
            public int StudentMark { get; set; }
            [JsonProperty("presence")]
            public bool Presence { get; set; }
            [JsonProperty("comment")]
            public string Comment { get; set; }
        }
    }
}
