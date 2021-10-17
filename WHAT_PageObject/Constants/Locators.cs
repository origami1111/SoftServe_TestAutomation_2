using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_PageObject
{
    public static class Locators
    {
        public static class MentorsPage
        {
            public const string DISABLED_MENTORS_TOGGLE = "//input[@type ='checkbox']";
            public const string SEARCH_FIELD = "//input[@type='text']";
            public const string ROW_AMOUNT_DROPDOWN = "//select";
            public const string SWITCH_TO_CARDS = "//*[contains(@href,'Card.svg')]/parent::*/parent::button";
            public const string SWITCH_TO_TABLE = "//*[contains(@href,'List.svg')]/parent::*/parent::button";
            public const string ADD_MENTOR_BUTTON = "//span[text()='Add a mentor']/parent::button";
            public const string PREVIOUS_PAGE_TOP_BUTTON = "//h2[text()='Mentors']/parent::div//button[text()='<']";
            public const string NEXT_PAGE_TOP_BUTTON = "//h2[text()='Mentors']/parent::div//button[text()='>']";
            public const string PREVIOUS_PAGE_BOTTOM_BUTTON = "//div[@class='row mr-0']//button[text()='<']";
            public const string NEXT_PAGE_BOTTOM_BUTTON = "//div[@class='row mr-0']//button[text()='>']";
            public const string SORTING_FIRST_NAME = "//span[@data-sorting-param='firstName']";
            public const string SORTING_LAST_NAME = "//span[@data-sorting-param='lastName']";
            public const string SORTING_EMAIL = "//span[@data-sorting-param='email']";
            public const string MENTORS_TABLE = "//table";
            public const string MENTORS_COUNT = "//span[text()[4]=' mentors']";
            public const string ALL_FIRST_NAMES = "//td[1]";
            public const string ALL_LAST_NAMES = "//td[2]";
            public const string ALL_EMAILS = "//td[3]";
        }

        public static class MentorDetailsPage
        {
            public const string EDIT_MENTOR_LINK = "Edit a mentor";
            public const string FIRST_NAME = "//span[@data-testid='firstName']";
            public const string LAST_NAME = "//span[@data-testid='lastName']";
            public const string EMAIL = "//span[@data-testid='email']";
            public const string MENTOR_DETAILS = "//div[@data-testid='mentorDetails']";
        }

        public static class EditMentorDetailsPage
        {
            public const string FORM = "//form";
            public const string MENTOR_DETAILS_LINK = "Mentor details";
            public const string FIRST_NAME = "//input[@id='firstName']";
            public const string LAST_NAME = "//input[@id='lastName']";
            public const string EMAIL = "//input[@id='email']";
            public const string GROUPS = "//input[@id='groupsInput']";
            public const string ADD_GROUP_BUTTON = "//button[@id='addGroup']";
            public const string COURSES = "//button[@id='coursesInput']";
            public const string ADD_COURSE_BUTTON = "//button[@id='addCourse']";
            public const string RESET_BUTTON = "//button[@id='resetBtn']";
            public const string SAVE_BUTTON = "//button[text()='Save']";
            public const string DISABLE_BUTTON = "//button[text()='Disable']";
            public const string FIRST_NAME_ERROR = "//input[@id='firstName']/parent::div/div";
            public const string LAST_NAME_ERROR = "//input[@id='lastName']/parent::div/div";
            public const string EMAIL_ERROR = "//input[@id='email']/parent::div/div";
        }
    }
}
