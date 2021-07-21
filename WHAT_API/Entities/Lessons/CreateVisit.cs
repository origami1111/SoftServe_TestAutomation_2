using Newtonsoft.Json;

namespace WHAT_API.Entities.Lessons
{
    public class CreateVisit
    {
        [JsonProperty("studentId")]
        public int StudentId { get; set; }
        [JsonProperty("studentMark")]
        public object StudentMark { get; set; }
        [JsonProperty("presence")]
        public bool Presence { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }

        public CreateVisit WithStudentId(int studentId)
        {
            StudentId = studentId;
            return this;
        }

        public CreateVisit WithStudentMark(object studentMark)
        {
            StudentMark = studentMark;
            return this;
        }

        public CreateVisit WithPresence(bool presence)
        {
            Presence = presence;
            return this;
        }

        public CreateVisit WithComment(string comment)
        {
            Comment = comment;
            return this;
        }
    }
}
