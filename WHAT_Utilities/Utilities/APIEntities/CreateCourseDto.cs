using Newtonsoft.Json;

namespace WHAT_Utilities
{
    public class CreateCourseDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = StringGenerator.GenerateStringOfLetters(50);
    }
}
