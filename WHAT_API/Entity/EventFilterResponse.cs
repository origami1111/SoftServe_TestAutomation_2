using System;

namespace WHAT_API
{
    public class EventFilterResponse
    {
        public int id { get; set; }
        public int eventOccuranceId { get; set; }
        public int studentGroupId { get; set; }
        public int themeId { get; set; }
        public int mentorId { get; set; }
        public int lessonId { get; set; }
        public DateTime eventStart { get; set; }
        public DateTime eventFinish { get; set; }
        public override string ToString()
        {

            return $"{id}, {eventOccuranceId}, {studentGroupId}, " +
                $"{themeId}, {mentorId}, {lessonId}"+
                $"{eventStart}, {eventFinish}";
        }
    }
}
