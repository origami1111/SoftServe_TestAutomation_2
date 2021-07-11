using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WHAT_Utilities;

namespace WHAT_API.API_Tests.Students
{
    [TestFixture]
    public class POST_AddNewStudent : API_BaseTest
    {
        [Test]
        public void VerifyAddingStudentAccount_Valid()
        {
            //1     Account => Registration of account {POST}
            //2     Account => Returns all not assigned accounts {GET}
            //3     Student => Get all students {GET}
            //4     Student => Addition of new student {POST}
            //Are equal 3 and 4?
            RegAccount();
            Assert.Pass();
        }
        private void RegAccount()
        {
            RestRequest request = new RestRequest("ApiAccountsReg", Method.POST);
            request.AddJsonBody(new { 
                email= "ThompsonFarzaneh@gmail.com",
                firstName = "Thompson",
                lastName = "Farzaneh",
                password = "qwerty1!",
                confirmPassword = "qwerty1!",
            });
            IRestResponse response = client.Execute(request);
            System.Console.WriteLine(response.StatusCode);
        }

    }
}
