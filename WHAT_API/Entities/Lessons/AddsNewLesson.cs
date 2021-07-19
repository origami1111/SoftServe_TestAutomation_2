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
        public List<Lessonvisit> LessonVisits { get; set; }
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

        public AddsNewLesson WithLessonVisits(List<Lessonvisit> lessonVisits)
        {
            LessonVisits = lessonVisits;
            return this;
        }
        
        public AddsNewLesson WithLessonDate(string lessonDate)
        {
            LessonDate = lessonDate;
            return this;
        }

        public class Lessonvisit
        {
            [JsonProperty("studentId")]
            public int StudentId { get; set; }
            [JsonProperty("studentMark")]
            public object StudentMark { get; set; }
            [JsonProperty("presence")]
            public bool Presence { get; set; }
            [JsonProperty("comment")]
            public string Comment { get; set; }

            public Lessonvisit WithStudentId(int studentId)
            {
                StudentId = studentId;
                return this;
            }
            
            public Lessonvisit WithStudentMark(object studentMark)
            {
                StudentMark = studentMark;
                return this;
            }
            
            public Lessonvisit WithPresence(bool presence)
            {
                Presence = presence;
                return this;
            }
            
            public Lessonvisit WithComment(string comment)
            {
                Comment = comment;
                return this;
            }
        }
    }
}
