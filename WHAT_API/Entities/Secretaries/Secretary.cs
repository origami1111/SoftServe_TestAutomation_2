using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_API.Entities.Secretaries
{
    class Secretary
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            Account other = (Account)obj;

            return (this.Email == other.Email
                && this.FirstName == other.FirstName
                && this.LastName == other.LastName);
        }
    }
}
