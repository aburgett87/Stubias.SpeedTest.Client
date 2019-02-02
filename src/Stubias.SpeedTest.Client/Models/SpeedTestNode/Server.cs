using Newtonsoft.Json;

namespace Stubias.SpeedTest.Client.Models.SpeedTestNode
{
    public class Server
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("cc")]
        public string Cc { get; set; }

        [JsonProperty("sponsor")]
        public string Sponsor { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("distanceMi")]
        public double DistanceMi { get; set; }

        [JsonProperty("ping")]
        public double Ping { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}