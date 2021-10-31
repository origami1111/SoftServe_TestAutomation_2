using System.Collections.Generic;

namespace WHAT_Utilities
{
    public class UpdateMentorDto
    {
        public string Email { get; set; } = StringGenerator.GenerateEmail();
        public string FirstName { get; set; } = StringGenerator.GenerateStringOfLetters(30);
        public string LastName { get; set; } = StringGenerator.GenerateStringOfLetters(30);
        public List<int> CourseIds { get; set; }
        public List<int> StudentGroupIds { get; set; }
    }
}
