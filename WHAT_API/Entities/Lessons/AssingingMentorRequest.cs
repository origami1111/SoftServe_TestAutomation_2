using Newtonsoft.Json;

namespace WHAT_API.Entities.Lessons
{
    public class AssingingMentorRequest
    {
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("lessonId")]
        public int LessonId { get; set; }

        public AssingingMentorRequest WithMentorId(int mentorId)
        {
            MentorId = mentorId;
            return this;
        }

        public AssingingMentorRequest WithLessonId(int lessonId)
        {
            LessonId = lessonId;
            return this;
        }
    }
}
