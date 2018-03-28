using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public class Range
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("StyleClass")]
        public string StyleClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("Start")]
        public string Start { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("End")]
        public string End { get; set; }
    }
}