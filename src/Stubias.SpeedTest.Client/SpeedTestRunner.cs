using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeedTest;
using Stubias.SpeedTest.Client.HttpClients;
using Stubias.SpeedTest.Client.Models;
using Stubias.SpeedTest.Client.Models.Configuration;

namespace Stubias.SpeedTest.Client
{
    public class SpeedTestRunner : ISpeedTestRunner
    {
        private readonly ISpeedTestClient _speedTestClient;
        private readonly ILogger<SpeedTestRunner> _logger;
        private readonly ISpeedtestHttpClient _speedtestHttpClient;
        private readonly Runner _runnerOptions;

        public SpeedTestRunner(ISpeedtestHttpClient speedtestHttpClient,
            ISpeedTestClient speedTestClient,
            IOptions<Runner> runnerOptions,
            ILogger<SpeedTestRunner> logger)
        {
            _runnerOptions = runnerOptions.Value;
            _speedtestHttpClient = speedtestHttpClient;
            _speedTestClient = speedTestClient;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Running speed test...");
            var executionDateTime = DateTime.UtcNow;
            _logger.LogDebug("Getting speed test client settings...");
            var settings = _speedTestClient.GetSettings();
            _logger.LogDebug("Retrieved settings");
            var servers = settings.Servers;
            var serverEnumerator = servers.GetEnumerator();

            var testSuccessful = false;
            SpeedTestResult result = null;
            while(serverEnumerator.MoveNext() && !testSuccessful)
            {
                var server = serverEnumerator.Current;
                try
                {
                    _logger.LogDebug($"Testing server {server.Host} latency");
                    server.Latency = _speedTestClient.TestServerLatency(server);
                    _logger.LogDebug($"Latency test complete");
                    _logger.LogDebug("Testing doownload speed...");
                    var downloadSpeed = _speedTestClient.TestDownloadSpeed(server, _runnerOptions.ConcurrentDownloads);
                    _logger.LogDebug("Download test completed");
                    _logger.LogDebug("Testing upload speed...");
                    var uploadSpeed = _speedTestClient.TestUploadSpeed(server, _runnerOptions.ConcurrentUploads);
                    _logger.LogDebug("Upload test completed");
                    result = new SpeedTestResult
                    {
                        NodeName = _runnerOptions.NodeName,
                        Location = _runnerOptions.Location,
                        ExecutionDateTime = executionDateTime,
                        AverageDownloadSpeed = (decimal)downloadSpeed,
                        AverageUploadSpeed = (decimal)uploadSpeed,
                        MaximumDownloadSpeed = (decimal)downloadSpeed,
                        MaximumUploadSpeed = (decimal)uploadSpeed,
                        Latency = server.Latency,
                        TestServerName = server.Host
                    };
                    testSuccessful = true;
                }
                catch(HttpRequestException hre)
                {
                    var message = $"Error testing server {server.Host}. Skipping...";
                    if(_runnerOptions.LogExceptions)
                    {
                        _logger.LogError(hre, message);
                    }
                    else
                    {
                        _logger.LogError(message);
                    }
                }
            } 

            if(result != null)
            {
                _logger.LogDebug("Posting speed test results...");
                await _speedtestHttpClient.PostTestResult(result);
                _logger.LogDebug("Speed test results posted");
                _logger.LogInformation("Speed test completed");
            }
            else
            {
                _logger.LogError("All speed tests failed");
            }
        }
    }
}