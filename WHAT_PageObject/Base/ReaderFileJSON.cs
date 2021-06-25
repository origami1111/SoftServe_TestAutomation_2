using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WHAT_PageObject.Base
{
    public class ReaderFileJson
    {
        private ReaderFileJson() { }

        public static Credentials ReadFileJsonCredentials(string path, Role role)
        {
            Credentials credentials = new Credentials();

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<Credentials> creds = JsonConvert.DeserializeObject<List<Credentials>>(json);

                credentials = creds.Where(x => x.Role.Equals(role)).FirstOrDefault();
            }

            return credentials;
        }

        public static List<Credentials> ReadFileJsonListCredentials(string path, Role role)
        {
            List<Credentials> credentials = new List<Credentials>();

            using(StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<Credentials> creds = JsonConvert.DeserializeObject<List<Credentials>>(json);

                credentials = creds.Where(x => x.Role.Equals(role)).ToList();
            }

            return credentials;
        }
    }
}
