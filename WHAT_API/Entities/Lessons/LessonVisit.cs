using Newtonsoft.Json;

namespace WHAT_API
{
    public class LessonVisit
    {
        [JsonProperty("studentId")]
        public int StudentId { get; set; }
        [JsonProperty("studentMark")]
        public int? StudentMark { get; set; }
        [JsonProperty("presence")]
        public bool Presence { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
