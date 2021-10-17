using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_PageObject
{
    public static class AssertionMessages
    {
        public static class EditMentorsDetailsPage
        {
            public static string FIRST_NAME = "'First name' field verification";
            public static string LAST_NAME = "'Last name' field verification";
            public static string EMAIL = "'Email' field verification";
            public static string FIRST_NAME_ERROR_MESSAGE = "'First name' field error verification";
            public static string LAST_NAME_ERROR_MESSAGE = "'Last name' field error verification";
            public static string EMAIL_ERROR_MESSAGE = "'Email' field error verification";
            public static string SAVE_BUTTON_ENABLED_MESSAGE = "'Save' button enabled state verification";

        }

        public static class MentorsPage
        {
            public static string FIRST_NAME_ASC_SORT = "First name sorting verification (ascending)";
            public static string FIRST_NAME_DESC_SORT = "First name sorting verification (descending)";
            public static string LAST_NAME_ASC_SORT = "Last name sorting verification (ascending)";
            public static string LAST_NAME_DESC_SORT = "Last name sorting verification (descending)";
            public static string EMAIL_ASC_SORT = "Email sorting verification (ascending)";
            public static string EMAIL_DESC_SORT = "Email sorting verification (descending)";
            public static string FIRST_NAME = "First name in table verification";
            public static string LAST_NAME = "Last name in table verification";
            public static string EMAIL = "Email in table verification";
        }

        public static class MentorDetailsPage
        {
            public static string FIRST_NAME = "'First name' field verification";
            public static string LAST_NAME = "'Last name' field verification";
            public static string EMAIL = "'Email' field verification";
        }
    }
}
