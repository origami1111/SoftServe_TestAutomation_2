using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_Tests
{
    public static class WarningMessagesData
    {
        public static string FirstName = "first name",
                             LastName = "last name",
                             Email = "email address";

        public static string WarningMessages(string data, string fieldName)
        {

            string message;

            if (data.Length < 2 && fieldName != Email)
            {
                message = "Too short";
            }
            else if (data.Length > 50 && fieldName != Email)
            {
                message = "Too long";
            }
            else
            {
                message = "Invalid " + fieldName;
            }

            return message;

        }
    }
}
