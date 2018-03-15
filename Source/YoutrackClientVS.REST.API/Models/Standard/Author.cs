using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class Author
    {
        [JsonProperty(PropertyName = "raw")]
        public string Raw { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserShort User { get; set; }
    }
}