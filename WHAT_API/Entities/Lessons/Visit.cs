using Newtonsoft.Json;

namespace WHAT_API.Entities.Lessons
{
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
