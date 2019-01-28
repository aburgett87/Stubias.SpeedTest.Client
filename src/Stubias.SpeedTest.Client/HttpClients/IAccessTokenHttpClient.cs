using System.Threading.Tasks;

namespace Stubias.SpeedTest.Client.HttpClients
{
    public interface IAccessTokenHttpClient
    {
        Task<string> GetToken();
    }
}