using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Stubias.SpeedTest.Client.Constants;
using Stubias.SpeedTest.Client.Models.Configuration;

namespace Stubias.SpeedTest.Client
{
    public class SpeedTestRunner : ISpeedTestRunner
    {
        private readonly ISpeedTestNetClient _speedTestNetRunner;
        private readonly Runner _runnerOptions;
        public SpeedTestRunner(IOptions<Runner> runnerOptions,
            ISpeedTestNetClient speedTestNetRunner)
        {
            _runnerOptions = runnerOptions.Value;
            _speedTestNetRunner = speedTestNetRunner;
        }

        public async Task RunAsync()
        {
            if(string.IsNullOrWhiteSpace(_runnerOptions.Client))
            {
                throw new ArgumentNullException("A client must be specified");
            }

            switch(_runnerOptions.Client)
            {
                case SpeedTestClientNames.SpeedTestNet:
                    await _speedTestNetRunner.RunAsync();
                    break;
                default:
                    throw new InvalidOperationException($"client {_runnerOptions.Client} is invalid");
            }
        }
    }
}