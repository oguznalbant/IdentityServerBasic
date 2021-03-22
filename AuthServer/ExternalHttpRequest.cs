using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Text.Json;

namespace AuthServer
{
    public static class ExternalHttpRequest
    {
        public static async void Request(string endpoint)
        {
            var httpClient = new HttpClient();
            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync("https://localhost:1000"); //auth server adresi
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = "GarantiBankasi",
                ClientSecret = "garanti",
                Address = discoveryDocumentResponse.TokenEndpoint, // connect/token adresi
            };

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.GetAsync("https://localhost:2000/api/garantibank/bakiye/3000");

            if (response.IsSuccessStatusCode)
            {
                int bakiye = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
                Console.WriteLine($"Hesap bakiyeniz : {bakiye}");
            }

        }

    }
}
