using System;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stubias.SpeedTest.Client.HttpClients;
using Stubias.SpeedTest.Client.Models;
using Stubias.SpeedTest.Client.Models.Configuration;
using Stubias.SpeedTest.Client.Models.SpeedTestNode;

namespace Stubias.SpeedTest.Client
{
    public class SpeedTestNodeClient : ISpeedTestNodeClient
    {
        private readonly ISpeedtestHttpClient _speedtestHttpClient;
        private readonly INodeServices _nodeServices;
        private readonly Runner _runnerOptions;
        private readonly ILogger<SpeedTestNodeClient> _logger;

        public SpeedTestNodeClient(ISpeedtestHttpClient speedtestHttpClient,
            INodeServices nodeServices,
            IOptions<Runner> runnerOptions,
            ILogger<SpeedTestNodeClient> logger)
        {
            _logger = logger;
            _runnerOptions = runnerOptions.Value;
            _speedtestHttpClient = speedtestHttpClient;
            _nodeServices = nodeServices;
        }

        public async Task RunAsync()
        {
            _logger.LogDebug("Running speed test with Node client...");
            var executionDateTime = DateTime.UtcNow;
            var nodeResult = await _nodeServices.InvokeExportAsync<SpeedTestNodeResult>("./Node/speedtest.js", "test");
            _logger.LogDebug("Speed test from Node client complete");

            _logger.LogDebug("Publishing results...");
            var result = new SpeedTestResult
            {
                NodeName = _runnerOptions.NodeName,
                Location = _runnerOptions.Location,
                ExecutionDateTime = executionDateTime,
                AverageDownloadSpeed = (decimal)nodeResult.Speeds.Download * 1000,
                AverageUploadSpeed = (decimal)nodeResult.Speeds.Upload * 1000,
                MaximumDownloadSpeed =  (decimal)nodeResult.Speeds.Download * 1000,
                MaximumUploadSpeed = (decimal)nodeResult.Speeds.Upload * 1000,
                Latency = (decimal)nodeResult.Server.Ping,
                TestServerName = nodeResult.Server.Host
            };
            await _speedtestHttpClient.PostTestResult(result);
            _logger.LogDebug("Results published");
        }
    }
}