﻿using Newtonsoft.Json;

namespace WHAT_API
{
    public class Secretaries
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}