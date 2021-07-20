using Newtonsoft.Json;

namespace WHAT_API.Entities.Lessons
{
    public class LessonVisit
    {
        [JsonProperty("studentId")]
        public int StudentId { get; set; }
        [JsonProperty("studentMark")]
        public object StudentMark { get; set; }
        [JsonProperty("presence")]
        public bool Presence { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }

        public LessonVisit WithStudentId(int studentId)
        {
            StudentId = studentId;
            return this;
        }

        public LessonVisit WithStudentMark(object studentMark)
        {
            StudentMark = studentMark;
            return this;
        }

        public LessonVisit WithPresence(bool presence)
        {
            Presence = presence;
            return this;
        }

        public LessonVisit WithComment(string comment)
        {
            Comment = comment;
            return this;
        }
    }
}
