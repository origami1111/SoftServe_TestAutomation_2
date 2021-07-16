using Newtonsoft.Json;
using WHAT_Utilities;

namespace WHAT_API
{
    class RegistrationResponseBody
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("isActive")]
        public Activity Activity { get; set; }
    }
}
