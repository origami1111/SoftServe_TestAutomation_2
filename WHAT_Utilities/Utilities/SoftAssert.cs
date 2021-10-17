using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WHAT_Utilities
{
    public class SoftAssert
    {
        List<object[]> assertions = new List<object[]>();

        public void Add(Object expected, Object actual, string message = "")
        {
            assertions.Add(new object[3]{expected, actual, message});
        }
        public void AssertAll()
        {
            Assert.Multiple(() =>
            {
                foreach (Object[] obs in assertions)
                {
                    string errorMessage = obs[2] as string;
                    Assert.AreEqual(obs[0], obs[1], errorMessage);
                }
            });
        }
    }
}
