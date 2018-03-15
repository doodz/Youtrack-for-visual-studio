using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseParent
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}