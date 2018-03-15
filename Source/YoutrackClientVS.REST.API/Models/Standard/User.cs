using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class User
    {
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "guest")]
        public string Guest { get; set; }
    }
}