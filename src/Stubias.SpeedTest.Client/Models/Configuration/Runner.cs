using System;

namespace Stubias.SpeedTest.Client.Models.Configuration
{
    public class Runner
    {
        public int Interval { get; set; }
        public string Location { get; set; }
        public string NodeName { get; set; } = Environment.MachineName;
        public int ConcurrentDownloads { get; set; }
        public int ConcurrentUploads { get; set; }
        public bool LogExceptions { get; set; }
    }
}