using System;
using System.Net.Http;
using IdentityModel.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using var client = new HttpClient();
            var discoveryDocumentResponse = client.GetDiscoveryDocumentAsync("http://localhost:5001");
            discoveryDocumentResponse.Wait();
            if (discoveryDocumentResponse.Result.IsError)
            {
                Console.WriteLine(discoveryDocumentResponse.Result.IsError);
                return;
            }

            var requestClientCredentialsTokenAsync = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocumentResponse.Result.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });
            requestClientCredentialsTokenAsync.Wait();
            if (requestClientCredentialsTokenAsync.Result.IsError)
            {
                Console.WriteLine(requestClientCredentialsTokenAsync.Result.IsError);
                return;
            } 

            Console.WriteLine(requestClientCredentialsTokenAsync.Result.Json);

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(requestClientCredentialsTokenAsync.Result.AccessToken);
            var response = apiClient.GetAsync("http://localhost:6001/identity");
            response.Wait();
            if (!response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Result.StatusCode);
            }
            else
            {
                var readAsStringAsync = response.Result.Content.ReadAsStringAsync();
                readAsStringAsync.Wait();
                Console.WriteLine(readAsStringAsync.Result);
            }
        }
    }
}
