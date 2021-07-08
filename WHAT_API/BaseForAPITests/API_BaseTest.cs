using NUnit.Framework;
using RestSharp;
using System.Linq;
using WHAT_Utilities;
namespace WHAT_API
{
    [SetUpFixture]
    public abstract class API_BaseTest
    {
        protected RestClient client;
        public readonly string endpointsPath = @"DateFiles/Endpoints.json";
        private readonly string linksPath = @"DateFiles/Links.json";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            client = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
        }

        public string GetToken(string email, string password)
        {
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { email, password });
            var response = client.Execute(request);
            string token = response.Headers.Where(h => h.Name == "Authorization")
                                           .Select(h => h.Value)
                                           .FirstOrDefault()
                                           .ToString();
            return token;
        }
    }
}
