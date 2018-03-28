namespace YouTrackClientVS.Contracts.Models.YouTrackClientModels
{
    public enum YouTrackStyleClass { Keyword, Field };

    public enum YouTrackSuffix { Empty };

    public class YouTrackSuggestItem
    {

        public YouTrackStyleClass StyleClass { get; set; }


        public string Prefix { get; set; }


        public string Option { get; set; }


        public object Complete { get; set; }


        public string Suffix { get; set; }


        public string Description { get; set; }


        public object HtmlDescription { get; set; }


        public long Caret { get; set; }


        public YouTrackCompletion Completion { get; set; }


        public YouTrackCompletion Match { get; set; }
    }
}