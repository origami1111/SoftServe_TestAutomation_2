using System.Collections.Generic;

namespace WHAT_Utilities
{
    public class CreateStudentGroupDto
    {
        public string Name { get; set; } = StringGenerator.GenerateStringOfLetters(50);
        public int CourseId { get; set; }
        public string StartDate { get; set; } = "2021-10-31T14:12:28.942Z";
        public string FinishDate { get; set; } = "2021-10-31T14:12:28.942Z";
        public List<int> StudentIds { get; set; }
        public List<int> MentorIds { get; set; }
    }
}
