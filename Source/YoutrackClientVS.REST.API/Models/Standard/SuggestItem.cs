using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public enum StyleClass { Keyword, Field };

    public enum Suffix { Empty };

    public class Completion
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }
    }
    /// <summary>
    ///  A class that represents YouTrack suggested item
    /// </summary>
    public class SuggestItem
    {
        [JsonProperty("styleClass")]
        public StyleClass StyleClass { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("option")]
        public string Option { get; set; }

        [JsonProperty("complete")]
        public object Complete { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("htmlDescription")]
        public object HtmlDescription { get; set; }

        [JsonProperty("caret")]
        public long Caret { get; set; }

        [JsonProperty("completion")]
        public Completion Completion { get; set; }

        [JsonProperty("match")]
        public Completion Match { get; set; }
    }




}