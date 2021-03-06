﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stubias.SpeedTest.Client.Models.Configuration;

namespace Stubias.SpeedTest.Client
{
    public class SpeedTestHostedService : IHostedService, IDisposable
    {
        private readonly Runner _runnerOptions;
        private readonly ILogger<SpeedTestHostedService> _logger;
        private Timer _timer;
        private readonly IServiceProvider _services;

        public SpeedTestHostedService(IServiceProvider services,
            IOptions<Runner> runnerOptions,
            ILogger<SpeedTestHostedService> logger)
        {
            _services = services;
            _runnerOptions = runnerOptions.Value;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting speed test runner");
            _logger.LogInformation($"Tests will run every {_runnerOptions.Interval} seconds");

            _timer = new Timer(async e => await RunTest(e),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(_runnerOptions.Interval));

            return Task.CompletedTask;
        }

        private async Task RunTest(object state)
        {
            using(var scope = _services.CreateScope())
            {
                var serviceRunner = scope.ServiceProvider.GetRequiredService<ISpeedTestRunner>();
                _logger.LogInformation("Running a speed test");
                await serviceRunner.RunAsync();
                _logger.LogInformation("Completed a speed test");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping speed test runner");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}