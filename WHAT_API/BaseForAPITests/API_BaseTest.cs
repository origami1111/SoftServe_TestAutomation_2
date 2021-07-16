using NLog;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
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
        protected Logger log= LogManager.GetCurrentClassLogger();
        protected readonly string endpointsPath = @"DataFiles/Endpoints.json";
        protected readonly string linksPath = @"DataFiles/Links.json";

        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            client = new RestClient(ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath));
            log.Info($"Go to BaseURLforAPI => {ReaderUrlsJSON.ByName("BaseURLforAPI", linksPath)}");
        }

        protected string GetToken(Role role)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                log.Info($"Sccesfully get toke by role {role}");
                return response.Headers.Single(h => h.Name == "Authorization").Value.ToString();
            }
            else
            {
                log.Error("Authorization is failed!");
                throw new Exception();
            }
        }
        protected string GetToken(Role role, RestClient client)
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(role);
            var request = new RestRequest(ReaderUrlsJSON.ByName("ApiAccountsAuth", endpointsPath), Method.POST);
            request.AddJsonBody(new { credentials.Email, credentials.Password });
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                log.Info($"Sccesfully get toke by role {role}");
                return response.Headers.Single(h => h.Name == "Authorization").Value.ToString();
            }
            else
            {
                log.Error("Authorization is failed!");
                throw new Exception();
            }
        }

        protected IAuthenticator GetAuthenticatorFor(Role role)
        {
            var accessToken = GetToken(role);
            if (accessToken.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                accessToken = accessToken.Substring("Bearer ".Length);
            }
            return new JwtAuthenticator(accessToken);
        }

        protected RestRequest InitNewRequest(string endPointName, Method method,
            IAuthenticator authenticator)
        {
            var resource = ReaderUrlsJSON.ByName(endPointName, endpointsPath);
            var request = new RestRequest(resource, method);
            authenticator.Authenticate(client, request);
            return request;
        }

        protected T Execute<T>(RestRequest request) where T : new()
        {
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var exception = new Exception(message, response.ErrorException);
                throw exception;
            }
            System.Diagnostics.Debug.WriteLine(response.Content);
            return response.Data;
        }
    }
}
