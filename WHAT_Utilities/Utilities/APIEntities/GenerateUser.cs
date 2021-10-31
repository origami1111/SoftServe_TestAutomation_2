using System;

namespace WHAT_Utilities
{
    public class GenerateUser
    {
        public string Email { get; set; } = $"{Guid.NewGuid():N}@gmail.com"; // random email
        public string FirstName { get; set; } = "Test";
        public string LastName { get; set; } = "Registration";
        public string Password { get; set; } = "Qwerty_123";
        public string ConfirmPassword { get; set; } = "Qwerty_123";
    }
}
