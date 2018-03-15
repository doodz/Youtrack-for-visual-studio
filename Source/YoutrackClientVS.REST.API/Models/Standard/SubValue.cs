using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    /// <summary>
    /// Represents a sub value which contains a <see cref="TSubValueType"/> value.
    /// </summary>
    public struct SubValue<TSubValueType>
    {
        /// <summary>
        /// Value.
        /// </summary>
        [JsonProperty("value")]
        public TSubValueType Value;
    }
}