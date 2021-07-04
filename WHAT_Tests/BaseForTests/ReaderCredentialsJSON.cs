using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WHAT_Tests
{
    public class ReaderCredentialsJSON
    {
        private const string path = @"Credentials\Credentials1.json";

        public static Credentials ReadFileJsonCredentials(Role role, Activity activity = Activity.Active)
        {
            string json = File.ReadAllText(path);

            var jsonParseOptions = new JsonSerializerOptions
            { 
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new JsonStringEnumConverter() }
            };
            var creds = JsonSerializer.Deserialize<List<Credentials>>(json, jsonParseOptions);

            var credentials = creds.Where(x => x.Role == role && x.Activity == activity)
                                   .FirstOrDefault();

            return credentials ?? new Credentials();
        }
    }
}
