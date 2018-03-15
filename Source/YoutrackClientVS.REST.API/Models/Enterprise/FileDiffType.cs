using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YouTrack.REST.API.Models.Enterprise
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileDiffType
    {
        [EnumMember(Value = "FROM")]
        From,
        [EnumMember(Value = "TO")]
        To
    }
}