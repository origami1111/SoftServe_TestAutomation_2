using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WHAT_Utilities
{
    public class ReaderFileJson
    {
        private ReaderFileJson() { }
        private const string path = @"DataFiles\Accounts.json";

        public static Account ReadFileJsonAccounts(Role role, Activity activity = Activity.Active)
        {
            string json = File.ReadAllText(path);
            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(json);

            Account account = accounts.Where(x => x.Role.Equals(role) && x.Activity.Equals(activity)).FirstOrDefault();
            return account;
        }

        public static List<Account> ReadFileJsonListAccounts(Role role, Activity activity = Activity.Active)
        {
            string json = File.ReadAllText(path);
            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(json);

            return accounts.Where(x => x.Role.Equals(role) && x.Activity.Equals(activity)).ToList();
        }

    }
}
