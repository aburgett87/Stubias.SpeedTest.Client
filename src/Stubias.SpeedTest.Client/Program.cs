using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeedTest;
using Stubias.SpeedTest.Client.HttpClients;
using Stubias.SpeedTest.Client.Models.Configuration;
using static System.Environment;

namespace Stubias.SpeedTest.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var homeDir = Environment.GetFolderPath(SpecialFolder.UserProfile);
                    var configFilePath =
                        $"{homeDir}{Path.DirectorySeparatorChar}.speedtest{Path.DirectorySeparatorChar}config.json";
                    config.AddJsonFile("appsettings.json");
                    config.AddUserSecrets<Program>(true);
                    config.AddEnvironmentVariables();
                    config.AddJsonFile(configFilePath, optional: true);

                    if(args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<Auth>(hostContext.Configuration.GetSection("Auth"));
                    services.Configure<Runner>(hostContext.Configuration.GetSection("Runner"));
                    services.Configure<Api>(hostContext.Configuration.GetSection("Api"));
                    services.AddHttpClient<ISpeedtestHttpClient, SpeedtestHttpClient>();
                    services.AddHttpClient<IAccessTokenHttpClient, AccessTokenHttpClient>();
                    services.AddTransient<ISpeedTestClient, SpeedTestClient>();
                    services.AddHostedService<SpeedTestHostedService>();
                    services.AddSingleton<ISpeedTestRunner, SpeedTestRunner>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

                await builder.RunConsoleAsync();
        }
    }
}
