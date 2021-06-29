using NUnit.Framework.Internal;
using System;

namespace WHAT_Tests
{
    internal class StringRandomizer
    {
        private const string AllowedRandomChars = " abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        
        private static readonly Randomizer randomizer = Randomizer.CreateRandomizer();

        public static string GetRandomCourseName()
        {
            return GetRandomString(AllowedRandomChars, 2, 50, false);
        }

        public static string GetRandomString(string allowedChars,
            byte minLength, byte maxLength, bool allowedDoubleSpaces = false)
        {
            var randomLength = randomizer.NextByte(minLength, maxLength);
            var text = randomizer.GetString(randomLength, allowedChars).Trim();
            
            if (!allowedDoubleSpaces && text.Contains("  "))
            {
                var substrings = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                text = string.Join(' ', substrings);
            }
            
            return text;
        }
    }
}
