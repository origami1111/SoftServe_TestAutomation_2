using NUnit.Framework;
using RestSharp;
using System;
using WHAT_Utilities;

namespace WHAT_API
{
    [TestFixture]
    public class POST_ReturnsEventrsList_InvalidRole : API_BaseTest
    {
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