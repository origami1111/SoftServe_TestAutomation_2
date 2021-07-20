using Newtonsoft.Json;

namespace WHAT_API.Entities.Lessons
{
    public class AssignMentorToLesson
    {
        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        [JsonProperty("lessonId")]
        public int LessonId { get; set; }

        public AssignMentorToLesson WithMentorId(int mentorId)
        {
            MentorId = mentorId;
            return this;
        }

        public AssignMentorToLesson WithLessonId(int lessonId)
        {
            LessonId = lessonId;
            return this;
        }
    }
}
