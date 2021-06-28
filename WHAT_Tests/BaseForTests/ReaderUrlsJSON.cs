using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WHAT_PageObject
{
    public class ReaderUrlsJSON
    {
        public ReaderUrlsJSON() { }

        private static string path = @"Links/Links.json";

        public static Uri ByName(string name)
        {
            NameAndUrl nameAndUrl = new NameAndUrl();
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                var urls = JsonConvert.DeserializeObject<List<NameAndUrl>>(json);


                nameAndUrl = urls.Where(x => x.Name.Equals(name)).FirstOrDefault();
            }
            return nameAndUrl.Url;
        }

        public static Uri ByNameAndNumber(string name, uint userNumber)
        {
            NameAndUrl nameAndUrl = new NameAndUrl();
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                var urls = JsonConvert.DeserializeObject<List<NameAndUrl>>(json);


                nameAndUrl = urls.Where(x => x.Name.Equals(name)).FirstOrDefault();
            }
            Uri urlAndUserNum = new Uri(nameAndUrl.Url.ToString() + "/" + userNumber.ToString(), UriKind.Absolute);
            return urlAndUserNum;
        }


    }
}
