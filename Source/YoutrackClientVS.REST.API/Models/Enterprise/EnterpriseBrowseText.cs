using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseBrowseText
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}