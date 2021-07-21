using System;
using System.Collections.Generic;

namespace WHAT_API
{
    public class CreateStudentGroup
    {
        public string Name { get; set; } = $"Test group {Guid.NewGuid().ToString("N")}"; // Required
        public long CourseId { get; set; } // Required
        public DateTime StartDate { get; set; } = DateTime.UtcNow; // Required
        public DateTime FinishDate { get; set; } = DateTime.UtcNow.AddMonths(1);// Required
        public IList<long> StudentIds { get; set; } = Array.Empty<long>(); // Required
        public IList<long> MentorIds { get; set; } = Array.Empty<long>(); // Required
    }
}
