using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseUser
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Uuid { get; set; }

        [JsonProperty(PropertyName = "links")]
        public EnterpriseLinks Links { get; set; }
    }
}