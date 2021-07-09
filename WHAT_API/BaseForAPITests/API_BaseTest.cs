using NUnit.Framework;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API
{
    [SetUpFixture]
    public abstract class API_BaseTest
    {
        protected RestClient client;
        public readonly string endpointsPath = @"DataFiles/Endpoints.json";
        private readonly string linksPath = @"DataFiles/Links.json";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            client = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
        }

        public string GetToken(Role role)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Headers.Where(h => h.Name == "Authorization")
                                           .Select(h => h.Value)
                                           .FirstOrDefault()
                                           .ToString();
            }
            else
            {
                throw new Exception("Authorization is failed!");
            }
        }
    }
}

