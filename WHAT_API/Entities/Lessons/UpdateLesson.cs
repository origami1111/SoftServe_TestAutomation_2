using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WHAT_API.Entities.Lessons
{
    public class UpdateLesson
    {
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }
        [JsonProperty("lessonDate")]
        public DateTime LessonDate { get; set; }
        [JsonProperty("lessonVisits")]
        public List<CreateVisit> LessonVisits { get; set; }

        public UpdateLesson WithThemaName(string themeName)
        {
            ThemeName = themeName;
            return this;
        }

        public UpdateLesson WithLessonDate(DateTime lessonDate)
        {
            LessonDate = lessonDate;
            return this;
        }

        public UpdateLesson WithLessonVisits(List<CreateVisit> lessonVisits)
        {
            LessonVisits = lessonVisits;
            return this;
        }
    }
}
