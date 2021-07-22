namespace WHAT_Tests.ChangePasswordTests
{
    public abstract class TestCasesChangePassword
    {
        public static object[] InvalidCurrentPass =
        {
        new object[] {"", "What_1234", "This field is required"}
        };

        public static object[] InvalidNewPass =
        {
        new object[] {"", "What_1234", "What_123", "This field is required"},
        new object[] {"111", "What_1234", "What_123", "Password must contain at least 8 characters"},
        new object[] {"11111111", "What_1234", "What_123", "Must contain at least one uppercase, one lowercase, one number"}
        };

        public static object[] InvalidConfirmPass =
        {
        new object[] {"11111111", "What_123", "What_1234", "You should confirm your password"},
        new object[] {"", "What_123", "What_1234", "This field is required"}
        };

        public static object[] CancelChangePassword =
        {
        new object[] {"What_123", "What_1234"}
        };
    }
}
