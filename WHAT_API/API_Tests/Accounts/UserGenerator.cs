using System;
using WHAT_API.Entities;

namespace WHAT_API.API_Tests
{
    public class UserGenerator
    {
        public static RegistrationRequestBody GenerateUser()
        {
            return new RegistrationRequestBody()
            {
                Email = $"{Guid.NewGuid():N}@gmail.com", // random email
                FirstName = "Test",
                LastName = "Registration",
                Password = "Qwerty_123",
                ConfirmPassword = "Qwerty_123"
            };
        }
    }
}
