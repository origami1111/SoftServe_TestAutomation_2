using Newtonsoft.Json;
using System.Collections.Generic;

namespace WHAT_API.Entities.Lessons
{
    public class CreateLesson
    {
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("studentGroupId")]
        public int StudentGroupId { get; set; }
        [JsonProperty("lessonVisits")]
        public List<CreateVisit> LessonVisits { get; set; }
        [JsonProperty("lessonDate")]
        public string LessonDate { get; set; }

        public CreateLesson WithThemaName(string themeName)
        {
            ThemeName = themeName;
            return this;
        }

        public CreateLesson WithMentorId(int mentorId)
        {
            MentorId = mentorId;
            return this;
        }

        public CreateLesson WithStudentGroupId(int studentGroupId)
        {
            StudentGroupId = studentGroupId;
            return this;
        }

        public CreateLesson WithLessonVisits(List<CreateVisit> lessonVisits)
        {
            LessonVisits = lessonVisits;
            return this;
        }

        public CreateLesson WithLessonDate(string lessonDate)
        {
            LessonDate = lessonDate;
            return this;
        }
    }
}

