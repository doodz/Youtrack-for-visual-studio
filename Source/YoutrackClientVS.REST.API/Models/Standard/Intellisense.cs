using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrack.REST.API.Models.Standard
{
    public class Intellisense
    {
        [JsonProperty("suggest")]
        public List<SuggestItem> Suggest { get; set; }

        [JsonProperty("recent")]
        public List<object> Recent { get; set; }

        [JsonProperty("highlight")]
        public List<Highlight> Highlight { get; set; }
    }
}