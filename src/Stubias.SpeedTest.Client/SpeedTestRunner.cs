using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Stubias.SpeedTest.Client.Constants;
using Stubias.SpeedTest.Client.Models.Configuration;

namespace Stubias.SpeedTest.Client
{
    public class SpeedTestRunner : ISpeedTestRunner
    {
        private readonly ISpeedTestNetClient _speedTestNetClient;
        private readonly Runner _runnerOptions;
        private readonly ISpeedTestNodeClient _speedTestNodeClient;

        public SpeedTestRunner(IOptions<Runner> runnerOptions,
            ISpeedTestNetClient speedTestNetRunner,
            ISpeedTestNodeClient speedTestNodeClient)
        {
            _speedTestNodeClient = speedTestNodeClient;
            _runnerOptions = runnerOptions.Value;
            _speedTestNetClient = speedTestNetRunner;
        }

    public async Task RunAsync()
    {
        if (string.IsNullOrWhiteSpace(_runnerOptions.Client))
        {
            throw new ArgumentNullException("A client must be specified");
        }

        switch (_runnerOptions.Client)
        {
            case SpeedTestClientNames.SpeedTestNet:
                await _speedTestNetClient.RunAsync();
                break;
            case SpeedTestClientNames.SpeedTestNode:
                await _speedTestNodeClient.RunAsync();
                break;
            default:
                throw new InvalidOperationException($"client {_runnerOptions.Client} is invalid");
        }
    }
}
}