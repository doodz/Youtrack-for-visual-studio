using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class Link
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}