using NUnit.Framework;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace WHAT_API
{
    [SetUpFixture]
    public abstract class API_BaseTest
    {
        protected RestClient client;
       
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            client = new RestClient("https://whatbackend.azurewebsites.net/api/");
        }

        public string GetToken(string email, string password)
        {
            string endPoint = "accounts/auth";
            var request = new RestRequest(endPoint, Method.POST);
            request.AddJsonBody(new { email, password });

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

