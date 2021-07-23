namespace WHAT_Tests.LessonsTests
{
    public abstract class TestCasesLessons
    {
        public static object[] AddLesson =
        {
        new object[] {"Test", "Advanced", "2021-06-28T09:00", "MentoR01@gmail.com", "The lesson has been added successfully!"}
        };

        public static object[] CancelEditLesson =
        {
        new object[] {"1", "nunit", "2021-06-29T08:00", "http://localhost:8080/lessons"}
        };

        public static object[] EditLesson =
        {
        new object[] {"1", "nunit", "2021-06-29T08:00", "The lesson has been edit successfully!"}
        };

        public static object[] SearchLesson =
{
        new object[] {"Junit", "1"},
        new object[] {"TestNG", "1"},
        new object[] {"API testing", "1"}
        };
    }
}
