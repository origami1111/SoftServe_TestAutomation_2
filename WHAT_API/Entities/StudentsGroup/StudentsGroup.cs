using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_API
{
    public class StudentsGroup
    {
            public int id { get; set; }
            public int courseId { get; set; }
            public string name { get; set; }
            public DateTime startDate { get; set; }
            public DateTime finishDate { get; set; }
            public List<int> studentIds { get; set; }
            public List<int> mentorIds { get; set; }
    }
}
