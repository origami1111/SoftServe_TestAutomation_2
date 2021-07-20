using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WHAT_API.Entities.Lessons
{
    public class ResponseAssingingMentor
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
        public Mentor mentor { get; set; }
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

        public class Mentor
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

        public class Visit
        {
            [JsonProperty("studentId")]
            public int StudentId { get; set; }
            [JsonProperty("lessonId")]
            public int LessonId { get; set; }
            [JsonProperty("studentMark")]
            public int? StudentMark { get; set; }
            [JsonProperty("presence")]
            public bool Presence { get; set; }
            [JsonProperty("comment")]
            public object Comment { get; set; }
            [JsonProperty("student")]
            public object Student { get; set; }
            [JsonProperty("id")]
            public int Id { get; set; }
        }
    }
}
