using Newtonsoft.Json;
using System.Collections.Generic;

namespace WHAT_API.Entities.Lessons
{
    public class UpdatesGivenLesson
    {
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }
        [JsonProperty("lessonDate")]
        public string LessonDate { get; set; }
        [JsonProperty("lessonVisits")]
        public List<LessonVisit> LessonVisits { get; set; }

        public UpdatesGivenLesson WithThemaName(string themeName)
        {
            ThemeName = themeName;
            return this;
        }

        public UpdatesGivenLesson WithLessonDate(string lessonDate)
        {
            LessonDate = lessonDate;
            return this;
        }

        public UpdatesGivenLesson WithLessonVisits(List<LessonVisit> lessonVisits)
        {
            LessonVisits = lessonVisits;
            return this;
        }
    }
}
