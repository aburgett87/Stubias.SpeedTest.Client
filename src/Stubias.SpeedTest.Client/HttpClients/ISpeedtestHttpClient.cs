using System.Threading.Tasks;
using Stubias.SpeedTest.Client.Models;

namespace Stubias.SpeedTest.Client.HttpClients
{
    public interface ISpeedtestHttpClient
    {
        Task PostTestResult(SpeedTestResult testResult);
    }
}