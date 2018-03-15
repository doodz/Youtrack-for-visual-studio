using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class Content
    {
        [JsonProperty(PropertyName = "raw")]
        public string Raw { get; set; }

        [JsonProperty(PropertyName = "markup")]
        public string Markup { get; set; }

        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }
    }
}