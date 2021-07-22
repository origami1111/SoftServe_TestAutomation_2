using Newtonsoft.Json;
using System.Collections.Generic;

namespace WHAT_API
{
    public class UpdateStudent
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("studentGroupIds")]
        public IList<long> StudentGroupIds { get; set; }
    }
}
