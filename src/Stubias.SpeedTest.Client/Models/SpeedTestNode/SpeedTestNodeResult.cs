using Newtonsoft.Json;

namespace Stubias.SpeedTest.Client.Models.SpeedTestNode
{
    public class SpeedTestNodeResult
    {
        [JsonProperty("speeds")]
        public Speeds Speeds { get; set; }

        [JsonProperty("client")]
        public Client Client { get; set; }

        [JsonProperty("server")]
        public Server Server { get; set; }
    }
}