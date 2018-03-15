using System;

namespace YouTrackClientVS.Contracts.Models
{
    public class YouTrackCredentials
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Uri Host { get; set; }
    }
}