using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using WHAT_PageObject;

namespace AutoLoginWHAT
{
    public class ReaderFileCSV
    {
        private ReaderFileCSV() { }

        public static List<Credentials> ReadFileListCredentials(string path)
        {
            List<Credentials> credentials = new List<Credentials>();

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip the row with the column names
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    credentials.Add(new Credentials
                    {
                        ID = int.Parse(fields[0]),
                        FirstName = fields[1],
                        LastName = fields[2],
                        Email = fields[3],
                        Password = fields[4],
                        Role = fields[5]
                    });
                }
            }

            return credentials;
        }

        public static Credentials ReadFileCredentials(string path, Role role)
        {
            List<Credentials> credentials = new List<Credentials>();
            Credentials credential = new Credentials();

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip the row with the column names
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    credentials.Add(new Credentials
                    {
                        ID = int.Parse(fields[0]),
                        FirstName = fields[1],
                        LastName = fields[2],
                        Email = fields[3],
                        Password = fields[4],
                        Role = fields[5]
                    });
                }

                credential = credentials[(int)role];
            }

            return credential;
        }
    }
}
