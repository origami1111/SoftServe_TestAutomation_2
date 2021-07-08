using Newtonsoft.Json;

namespace WHAT_API
{
    public class Context
    {
        [JsonProperty("groupID")] // Required
        public long GroupID { get; set; }

        [JsonProperty("themeID")]
        public long? ThemeID { get; set; }

        [JsonProperty("mentorID")]
        public long? MentorID { get; set; }
    }
}
