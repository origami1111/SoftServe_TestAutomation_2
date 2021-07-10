using Newtonsoft.Json;
using WHAT_Utilities;

namespace WHAT_API.Entities
{
    class SignInResponseBody
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }
    }
}
