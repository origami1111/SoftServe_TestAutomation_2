using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WHAT_Utilities
{
    public class ReaderFileJson
    {
        private ReaderFileJson() { }
        private const string path = @"DataFiles\Credentials.json";

        public static Credentials ReadFileJsonCredentials(Role role, Activity activity = Activity.Active)
        {
            string json = File.ReadAllText(path);
            List<Credentials> creds = JsonConvert.DeserializeObject<List<Credentials>>(json);

            return creds.Where(x => x.Role.Equals(role) && x.Activity.Equals(activity)).FirstOrDefault();
        }

    }
}
