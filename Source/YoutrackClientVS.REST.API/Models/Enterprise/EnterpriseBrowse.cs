using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseBrowsePage
    {
        [JsonProperty(PropertyName = "lines")]
        public List<EnterpriseBrowseText> Lines { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int? Limit { get; set; }

        [JsonProperty(PropertyName = "isLastPage")]
        public bool? IsLastPage { get; set; }

        [JsonProperty(PropertyName = "size")]
        public ulong? Size { get; set; }
    }
}
