namespace WHAT_Tests
{
    public class Lesson
    {
        private string lessonThema;
        private string groupName;
        private string dateTime;
        private string mentorEmail;

        public Lesson(string lessonThema, string groupName, string dateTime, string mentorEmail)
        {
            this.lessonThema = lessonThema;
            this.groupName = groupName;
            this.dateTime = dateTime;
            this.mentorEmail = mentorEmail;
        }

        public string GetLessonThema()
        {
            return lessonThema;
        }

        public string GetGroupName()
        {
            return groupName;
        }

        public string GetDateTime()
        {
            return dateTime;
        }

        public string GetMentorEmail()
        {
            return mentorEmail;
        }

    }
}
