using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseCommit
    {
        [JsonProperty(PropertyName = "author")]
        public EnterpriseUser Author { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "authorTimestamp")]
        public long Date { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}