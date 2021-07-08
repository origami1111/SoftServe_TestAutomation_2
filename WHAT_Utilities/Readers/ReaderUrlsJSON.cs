using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WHAT_Tests;
namespace WHAT_Utilities
{
    
    public class ReaderUrlsJSON
    {
        public ReaderUrlsJSON() { }


        public static Uri ByName(string name, string path)
        {
            NameAndUrl nameAndUrl = new NameAndUrl();
            var urls = JsonConvert.DeserializeObject<List<NameAndUrl>>(File.ReadAllText(path));
            nameAndUrl = urls.Where(x => x.Name.Equals(name)).FirstOrDefault();
            return nameAndUrl.Url;
        }

        public static Uri ByNameAndNumber(string name, int userNumber, string path)
        {
            Uri urlAndUserNum = new Uri(ByName(name, path).ToString() + "/" + userNumber.ToString(), UriKind.Absolute);
            return urlAndUserNum;
        }

        public static string GetUrlByName(string name, string path)
        {
            return ByName(name, path).ToString();
        }

        public static string GetUrlByNameAndNumber(string name, int userNumber, string path)
        {
            return ByNameAndNumber(name, userNumber, path).ToString();
        }
    }
}
