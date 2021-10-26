using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using WHAT_Utilities;

namespace WHAT_API
{
    public abstract class API_BaseTest
    {
        protected APIClient api = new APIClient();

        [SetUp]
        public void LogBeforeEachTest()
        {
            var context = TestContext.CurrentContext;
            var testName = context.Test.FullName;
            api.log.Info($"{testName} start ...");
        }

        [TearDown]
        public void LogAfterEachTest()
        {
            var context = TestContext.CurrentContext;
            var testName = context.Test.FullName;
            if (context.Result.Outcome.Status != TestStatus.Passed)
            {
                foreach (var assertion in context.Result.Assertions)
                {
                    api.log.Error($"{testName} {assertion.Status}:{Environment.NewLine}{assertion.Message}");
                }
            }
            api.log.Info($"{testName} {context.Result.Outcome.Status}" +
                $"{Environment.NewLine}----------------------------------------------------------");
        }
    }
}