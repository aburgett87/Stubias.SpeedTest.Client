using Newtonsoft.Json;

namespace Stubias.SpeedTest.Client.Models.SpeedTestNode
{
    public class Client
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }

        [JsonProperty("isprating")]
        public double Isprating { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("ispdlavg")]
        public long Ispdlavg { get; set; }

        [JsonProperty("ispulavg")]
        public long Ispulavg { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}