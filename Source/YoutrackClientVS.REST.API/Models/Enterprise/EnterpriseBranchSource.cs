using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseBranchSource : EnterpriseBranch
    {
        [JsonProperty(PropertyName = "repository")]
        public EnterpriseRepository Repository { get; set; }
    }
}