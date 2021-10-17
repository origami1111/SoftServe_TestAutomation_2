using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_Utilities
{
    public static class StringGenerator
    {
        public static string GenerateStringOfLetters(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            int asciiOffset = 97;
            int asciiRange = 25;

            char letter;

            for (int i = 0; i < length; i++)
            {
                int shift = Convert.ToInt32(Math.Floor(asciiRange * random.NextDouble()));
                letter = Convert.ToChar(shift + asciiOffset);
                stringBuilder.Append(letter);
            }
            return stringBuilder.ToString();
        }

        public static string GenerageEmail()
        {
            return $"{Guid.NewGuid():N}@example.com";
        }
    }
}
