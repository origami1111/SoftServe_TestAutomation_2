using NUnit.Framework;
using RestSharp;
using System.Linq;

namespace WHAT_API
{
    public class BaseTestAPI
    {
        public static string token;

        [OneTimeSetUp]
        public void GetToken_Admin()
        {
            RestClient client = new RestClient(ReaderUrlsJSON.ByName("ApiAccountsAuth"));
            RestRequest request = new RestRequest(Method.POST);
            request.AddJsonBody(new
            {
                email = "admin.@gmail.com",
                password = "admiN_12"
            });
            IRestResponse response = client.Execute(request);
            token = response.Headers.Where(h => h.Name == "Authorization").Select(h => h.Value).FirstOrDefault().ToString();
        }
    }
}
