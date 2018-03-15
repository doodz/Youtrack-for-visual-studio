using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class DefaultBranch
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
