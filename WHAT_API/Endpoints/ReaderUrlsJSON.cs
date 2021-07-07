using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WHAT_API
{
    public class ReaderUrlsJSON
    {
        public ReaderUrlsJSON() { }

        private static string path = @"Endpoints/Endpoints.json";

        public static Uri ByName(string name)
        {
            NameAndUrl nameAndUrl = new NameAndUrl();
            var urls = JsonConvert.DeserializeObject<List<NameAndUrl>>(File.ReadAllText(path));
            nameAndUrl = urls.Where(x => x.Name.Equals(name)).FirstOrDefault();
            return nameAndUrl.Url;
        }

        public static Uri ByNameAndNumber(string name, int userNumber)
        {
            Uri urlAndUserNum = new Uri(ByName(name).ToString() + "/" + userNumber.ToString(), UriKind.Absolute);
            return urlAndUserNum;
        }

        public static string GetUrlByName(string name)
        {
            return ByName(name).ToString();
        }

        public static string GetUrlByNameAndNumber(string name, int userNumber)
        {
            return ByNameAndNumber(name, userNumber).ToString();
        }
    }
}
