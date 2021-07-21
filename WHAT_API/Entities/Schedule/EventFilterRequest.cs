using System;

namespace WHAT_API
{
    public class EventFilterRequest
    {
        public int courseID { get; set; }
        public int mentorID { get; set; }
        public int groupID { get; set; }
        public int themeID { get; set; }
        public int studentAccountID { get; set; }
        public int eventOccurrenceID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime finishDate { get; set; }
    }
}
