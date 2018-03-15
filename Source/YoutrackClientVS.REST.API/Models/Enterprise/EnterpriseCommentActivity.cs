using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseCommentActivity : EnterpriseActivity
    {
        [JsonProperty(PropertyName = "comment")]
        public EnterpriseComment Comment { get; set; }

        [JsonProperty(PropertyName = "commentAnchor")]
        public EnterpriseAnchor Anchor { get; set; }

        [JsonProperty(PropertyName = "commentAction")]
        public string CommentAction { get; set; }
    }
}