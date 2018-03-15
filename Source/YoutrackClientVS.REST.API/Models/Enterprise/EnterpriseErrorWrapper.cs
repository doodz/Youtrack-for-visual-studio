using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Enterprise
{
    public class EnterpriseErrorWrapper
    {
        [JsonProperty(PropertyName = "errors")]
        public List<EnterpriseError> Errors { get; set; }
    }
}