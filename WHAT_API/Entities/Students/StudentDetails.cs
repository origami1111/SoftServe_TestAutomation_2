using Newtonsoft.Json;

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
    }
}
