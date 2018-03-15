using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class Participant
    {
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "approved")]
        public bool Approved { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserShort User { get; set; }
    }
}