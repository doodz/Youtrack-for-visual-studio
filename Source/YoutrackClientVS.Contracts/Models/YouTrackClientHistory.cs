using Newtonsoft.Json;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Contracts.Models
{
    public class YouTrackClientHistory
    {
        public static YouTrackClientHistory Default => new YouTrackClientHistory();
        [JsonProperty]
        public YouTrackProject ActiveProject { get; set; }
        [JsonProperty]
        public YouTrackProject PreviousProject { get; set; }
        [JsonProperty]
        public string LastFilter { get; set; }
    }
}