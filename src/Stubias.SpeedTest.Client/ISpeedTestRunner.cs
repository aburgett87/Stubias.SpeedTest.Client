using System.Threading.Tasks;

namespace Stubias.SpeedTest.Client
{
    public interface ISpeedTestRunner
    {
        Task RunAsync();
    }
}