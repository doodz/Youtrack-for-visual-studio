using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class Highlight
    {
        [JsonProperty("styleClass")]
        public string StyleClass { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }
    }
}