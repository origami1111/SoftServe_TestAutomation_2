using Newtonsoft.Json;

namespace WHAT_API
{
    public class Themes
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
