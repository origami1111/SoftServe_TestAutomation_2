using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;

namespace WHAT_API
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            RestClient client = new RestClient("https://whatbackend.azurewebsites.net/api/mentors");
            client.Authenticator = new SimpleAuthenticator("email","admin.@gmail.com","password", "admiN_12");
            RestRequest request = new RestRequest("resource",Method.GET);
            request.AddHeader("Accept", "text/json");
            //request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJNZW50b3IiLCJJZCI6IjEiLCJFbWFpbCI6Im1lbnRvckBnbWFpbC5jb20iLCJBY2NvdW50SWQiOiIzMyIsIm5iZiI6MTYyNTQ5MDI4NCwiZXhwIjoxNjI1NTMzNDg0LCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhDbGllbnQifQ.39oW7lsVjDleVgi4cyR340Mq3u4KVjqZTfKQOmaIiiY");
            //request.AddParameter("email", "admin.@gmail.com");
            //request.AddParameter("password", "admiN_12");

            IRestResponse response = client.Execute(request);

            string stream = response.Content;

            //var credentials = JsonConvert.DeserializeObject<List<User>>(stream);
            /*  var list = from s in credentials
                        where s.firstName == "Mentor"
                        select s;
           */
            int actual = (int)response.StatusCode;
            int expect = 200;
            Assert.AreEqual(expect, actual);
           
        }
    }
}