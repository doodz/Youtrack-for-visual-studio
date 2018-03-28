using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrack.REST.API.Models.Standard
{
    /// <summary>
    /// Wrapper for <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="SuggestItem" />.
    /// </summary>
    internal class SuggestItemCollectionWrapper
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="SuggestItem" />.
        /// </summary>
        [JsonProperty("IntelliSense")]
        public ICollection<SuggestItem> SuggestItems { get; set; }
    }
}