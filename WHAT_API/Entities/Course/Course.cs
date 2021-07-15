using System;
using System.Diagnostics.CodeAnalysis;

namespace WHAT_API
{
    public class Course : IEquatable<Course>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Equals([AllowNull] Course other)
        {
            return other != null
                && other.Id == Id
                && other.Name == Name
                && other.IsActive == IsActive;
        }
    }
}
