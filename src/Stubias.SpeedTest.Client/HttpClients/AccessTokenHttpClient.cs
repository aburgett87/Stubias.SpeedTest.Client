using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Stubias.SpeedTest.Client.Models.Configuration;

namespace Stubias.SpeedTest.Client.HttpClients
{
    public class AccessTokenHttpClient : IAccessTokenHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly Auth _authOptions;

        public AccessTokenHttpClient(HttpClient httpClient,
            IOptions<Auth> authOptions)
        {
            _httpClient = httpClient;
            _authOptions = authOptions.Value;
        }

        public async Task<string> GetToken()
        {
            var discoveryDoc = await _httpClient.GetDiscoveryDocumentAsync(_authOptions.Authority);
            var token = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDoc.TokenEndpoint,
                ClientId = _authOptions.ClientId,
                ClientSecret = _authOptions.ClientSecret,
                Scope = string.Join(' ', _authOptions.Scopes),
                Parameters = { { "audience", _authOptions.Audience } }
            });
            return token.AccessToken;
        }
    }
}