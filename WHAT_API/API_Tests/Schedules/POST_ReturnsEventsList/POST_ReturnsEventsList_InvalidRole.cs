using NLog;
using NUnit.Framework;
using RestSharp;
using System;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class POST_ReturnsEventsList_InvalidRole : API_BaseTest
    {
        public POST_ReturnsEventsList_InvalidRole()
        {
            log = LogManager.GetLogger($"Schedule/{nameof(POST_ReturnsEventsList_InvalidRole)}");
        }

        [Test]
        public void VerifyReturnsEventrsList_RoleUnassigned_Invalid([Values(Role.Unassigned)] Role user)
        {
            try
            {
                RestRequest request = new RestRequest(ReaderUrlsJSON.ByName("ApiSchedulesEvent", endpointsPath), Method.POST);
                request.AddHeader("Authorization", GetToken(user));
                Assert.Fail();
                log.Fatal("Not correct token, user is valid");
            }
            catch (Exception)
            {
                Assert.Pass();
                log.Info("Exception is cought and correctly handled");
            }
        }
    }
}