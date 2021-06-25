using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WHAT_PageObject
{
    public class ReaderUrls
    {
        private string _authority;
        private List<string> _pathList = new List<string>();
        private string _fileName;
        public static Dictionary<string, Uri> dictLinks = new Dictionary<string, Uri>();

        public ReaderUrls(string fileName= "whatURLs.csv")
        {
            _fileName = fileName;
            dictLinks = GetAllUrl();
        }

        private Dictionary<string, Uri> GetAllUrl()
        {

            _authority = File.ReadLines(_fileName).First();
            _pathList = File.ReadAllLines(_fileName).ToList<string>();

            foreach (var link in _pathList)
            {
                if (link != _authority)
                {
                    dictLinks.Add(link.Trim('/'), new Uri(_authority + link, UriKind.Relative));
                }
            }
            foreach (var item in dictLinks)
            { 
                Console.WriteLine($"{item.Key} , {item.Value}"); 
            }
            return dictLinks;

        }

        public static Uri GetCurrentUrl(string path)
        {
            foreach (var link in dictLinks)
            {

                if (link.Key == path)
                {
                    return link.Value;
                }
                else
                {
                    continue;
                }
            }
            return new Uri("0");
            
        }
            
    }
}
