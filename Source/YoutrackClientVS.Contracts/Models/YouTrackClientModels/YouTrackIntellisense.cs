using System.Collections.Generic;

namespace YouTrackClientVS.Contracts.Models.YouTrackClientModels
{
    public class YouTrackIntellisense
    {
        public List<YouTrackSuggestItem> Suggest { get; set; }


        public List<object> Recent { get; set; }


        public List<YouTrackHighlight> Highlight { get; set; }
    }
}