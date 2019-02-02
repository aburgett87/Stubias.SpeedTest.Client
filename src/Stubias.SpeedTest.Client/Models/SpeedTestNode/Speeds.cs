using Newtonsoft.Json;

namespace Stubias.SpeedTest.Client.Models.SpeedTestNode
{
    public class Speeds
    {
        [JsonProperty("download")]
        public double Download { get; set; }

        [JsonProperty("upload")]
        public double Upload { get; set; }

        [JsonProperty("originalDownload")]
        public long OriginalDownload { get; set; }

        [JsonProperty("originalUpload")]
        public long OriginalUpload { get; set; }
    }
}