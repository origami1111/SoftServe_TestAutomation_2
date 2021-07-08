using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WHAT_Utilities;

namespace WHAT_Tests
{
    public class ReaderFileJson
    {
        private ReaderFileJson() { }
        private const string path = @"Credentials\Credentials.json";

        public static Credentials ReadFileJsonCredentials(Role role, Activity activity = Activity.Active)
        {
            Credentials credentials = new Credentials();

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<Credentials> creds = JsonConvert.DeserializeObject<List<Credentials>>(json);
                
                credentials = creds.Where(x => x.Role.Equals(role) && x.Activity.Equals(activity)).FirstOrDefault();
            }

            return credentials;
        }

        public static List<Credentials> ReadFileJsonListCredentials(Role role, Activity activity = Activity.Active)
        {
            List<Credentials> credentials = new List<Credentials>();

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<Credentials> creds = JsonConvert.DeserializeObject<List<Credentials>>(json);

                credentials = creds.Where(x => x.Role.Equals(role) && x.Activity.Equals(activity)).ToList();

            }

            return credentials;
        }

    }
}
