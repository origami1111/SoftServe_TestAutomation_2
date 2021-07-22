using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Lessons
{
    public static class TestCase
    {
        public static object[] ValidRole =
        {
        new object[] {HttpStatusCode.OK, Role.Admin},
        new object[] {HttpStatusCode.OK, Role.Mentor},
        new object[] {HttpStatusCode.OK, Role.Secretary},
        new object[] {HttpStatusCode.OK, Role.Student }
        };

        public static object[] ValidAssingingMentorToLesson =
        {
        new object[] {HttpStatusCode.OK, Role.Admin},
        new object[] {HttpStatusCode.OK, Role.Secretary}
        };

        public static object[] ValidUpdatesGivenLesson =
        {
        new object[] {HttpStatusCode.OK, Role.Admin},
        new object[] {HttpStatusCode.OK, Role.Mentor}
        };

        public static object[] ForbiddenUpdatesGivenLesson =
        {
        new object[] {HttpStatusCode.Forbidden, Role.Secretary},
        new object[] {HttpStatusCode.Forbidden, Role.Student}
        };

        public static object[] ForbiddenAssingingMentorToLesson =
        {
        new object[] {HttpStatusCode.Forbidden, Role.Student},
        new object[] {HttpStatusCode.Forbidden, Role.Mentor}
        };

        public static object[] ValidForListOfAllLessons =
        {
        new object[] {HttpStatusCode.OK, Role.Admin, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment", 1},
        new object[] {HttpStatusCode.OK, Role.Mentor, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment", 1}
        };

        public static object[] ValidForAddsNewLesson =
        {
        new object[] {HttpStatusCode.OK, Role.Admin, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment"},
        new object[] {HttpStatusCode.OK, Role.Mentor, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment"}
        };

        public static object[] ForbiddenForAddsNewLesson =
        {
        new object[] {HttpStatusCode.Forbidden, Role.Student, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment"},
        new object[] {HttpStatusCode.Forbidden, Role.Secretary, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment"}
        };

        public static object[] UnauthorizedAddsNewLesson =
        {
        new object[] {HttpStatusCode.Unauthorized, "Some theme", 22, "2015-07-20T18:30:25", 5, true, "myComment"},
        };

        public static object[] BadRequestAddsNewLesson =
        {
        new object[] {HttpStatusCode.BadRequest, Role.Admin, "Some theme", 2, 22, "2015-07-20T18:30:25", 5, true, "myComment"},
        };
    }
}
