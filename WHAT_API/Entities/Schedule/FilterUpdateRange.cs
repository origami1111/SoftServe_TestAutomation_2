using Newtonsoft.Json;
using System;

namespace WHAT_API.Entity
{
    public class FilterUpdateRange
    {
        [JsonProperty("courseID")]
        public object CourseId { get; set; }
        [JsonProperty("mentorID")]
        public int MentorId { get; set; }
        [JsonProperty("groupID")]
        public object GroupId { get; set; }
        [JsonProperty("themeID")]
        public object ThemeId { get; set; }
        [JsonProperty("studentAccountID")]
        public object StudentAccountId { get; set; }
        [JsonProperty("eventOccurrenceID")]
        public object EventOccurrenceId { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("finishDate")]
        public DateTime FinishDate { get; set; }
        public FilterUpdateRange WithCourseId(object CourseId)
        {
            this.CourseId = CourseId;
            return this;
        }

        public FilterUpdateRange WithMentorId(int mentorId)
        {
            MentorId = mentorId;
            return this;
        }

        public FilterUpdateRange WithGroupId(object groupId)
        {
            GroupId = groupId;
            return this;
        }

        public FilterUpdateRange WithThemeId(object themeID)
        {
            ThemeId = themeID;
            return this;
        }

        public FilterUpdateRange WithSudentAccountId(object studentAccountId)
        {
            StudentAccountId = studentAccountId;
            return this;
        }

        public FilterUpdateRange WithEventOccurenceId(object eventOccurrenceId)
        {
            EventOccurrenceId = eventOccurrenceId;
            return this;
        }

        public FilterUpdateRange WithStartDate(DateTime startDate)
        {
            StartDate = startDate;
            return this;
        }

        public FilterUpdateRange WithFinishDate(DateTime finishDate)
        {
            FinishDate = finishDate;
            return this;
        }
    }
}
