using Newtonsoft.Json;
using System;

namespace WHAT_API
{
    public class StudentDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }

        public override bool Equals(object obj)
        {
            StudentDetails other = (StudentDetails)obj;

            return (this.Email == other.Email
                && this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.AvatarUrl == other.AvatarUrl);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
