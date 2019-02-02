using System;
using Stubias.SpeedTest.Client.Constants;

namespace Stubias.SpeedTest.Client.Models.Configuration
{
    public class Runner
    {
        public int Interval { get; set; }
        public string Location { get; set; }
        public string NodeName { get; set; } = Environment.MachineName;
        public SpeedTestNetClientConfiguration SpeedTestNetClient { get; set; }
        public bool LogExceptions { get; set; }
        public string Client { get; set; } = SpeedTestClientNames.SpeedTestNode;
    }
}