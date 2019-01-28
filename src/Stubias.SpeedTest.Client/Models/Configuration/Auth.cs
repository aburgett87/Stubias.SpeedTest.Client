using System.Collections.Generic;

namespace Stubias.SpeedTest.Client.Models.Configuration
{
    public class Auth
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public ICollection<string> Scopes { get; set; } = new List<string>();
    }
}