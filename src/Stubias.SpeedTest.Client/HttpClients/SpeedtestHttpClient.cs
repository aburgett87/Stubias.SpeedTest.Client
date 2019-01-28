using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Stubias.SpeedTest.Client.Models;
using Stubias.SpeedTest.Client.Models.Configuration;

namespace Stubias.SpeedTest.Client.HttpClients
{
    public class SpeedtestHttpClient : ISpeedtestHttpClient
    {
        private readonly IAccessTokenHttpClient _accessTokenHttpClient;
        private readonly Api _apiOptions;
        private readonly HttpClient _httpClient;

        public SpeedtestHttpClient(HttpClient httpClient,
            IAccessTokenHttpClient accessTokenHttpClient,
            IOptions<Api> apiOptions)
        {
            _httpClient = httpClient;
            _accessTokenHttpClient = accessTokenHttpClient;
            _apiOptions = apiOptions.Value;
        }

        public async Task PostTestResult(SpeedTestResult testResult)
        {
            var token = await _accessTokenHttpClient.GetToken();
            _httpClient.SetBearerToken(token);
            await _httpClient.PostAsJsonAsync($"{_apiOptions.BaseUrl}/api/results", testResult);
        }
    }
}