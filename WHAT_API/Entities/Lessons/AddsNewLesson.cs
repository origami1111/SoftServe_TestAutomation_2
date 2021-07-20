using Newtonsoft.Json;
using System.Collections.Generic;

namespace WHAT_API.Entities.Lessons
{
    public class AddsNewLesson
    {
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("studentGroupId")]
        public int StudentGroupId { get; set; }
        [JsonProperty("lessonVisits")]
        public List<LessonVisit> LessonVisits { get; set; }
        [JsonProperty("lessonDate")]
        public string LessonDate { get; set; }

        public AddsNewLesson WithThemaName(string themeName)
        {
            ThemeName = themeName;
            return this;
        }

        public AddsNewLesson WithMentorId(int mentorId)
        {
            MentorId = mentorId;
            return this;
        }

        public AddsNewLesson WithStudentGroupId(int studentGroupId)
        {
            StudentGroupId = studentGroupId;
            return this;
        }

        public AddsNewLesson WithLessonVisits(List<LessonVisit> lessonVisits)
        {
            LessonVisits = lessonVisits;
            return this;
        }

        public AddsNewLesson WithLessonDate(string lessonDate)
        {
            LessonDate = lessonDate;
            return this;
        }
    }
}

