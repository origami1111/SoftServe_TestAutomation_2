using Newtonsoft.Json;

namespace WHAT_API
{
    public class Secretary
    {
        [JsonProperty("id")]
        public int ID { get; set; }
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
