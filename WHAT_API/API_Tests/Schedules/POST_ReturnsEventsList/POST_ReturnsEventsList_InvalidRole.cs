using NLog;
using NUnit.Allure.Core;
using NUnit.Framework;
using RestSharp;
using System;
using WHAT_Utilities;

namespace WHAT_API
{
    [AllureNUnit]
    [TestFixture]
    public class POST_ReturnsEventsList_InvalidRole : API_BaseTest
    {
        public POST_ReturnsEventsList_InvalidRole()
        {
            api.log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_InvalidRole)}");
        }

        [Test]
        public void VerifyReturnsEventrsList_RoleUnassigned_Invalid([Values(Role.Unassigned)] Role role)
        {
            try
            {
                RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", api.endpointsPath), Method.POST);
                request.AddHeader("Authorization", api.GetToken(role));
                Assert.Fail();
                api.log.Fatal("Not correct token, user is valid");
            }
            catch (Exception)
            {
                Assert.Pass();
                api.log.Info("Exception is cought and correctly handled");
            }
        }
    }
}