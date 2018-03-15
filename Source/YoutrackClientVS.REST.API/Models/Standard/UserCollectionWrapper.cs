using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    /// <summary>
    /// Wrapper for <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="User" />.
    /// </summary>
    internal class UserCollectionWrapper
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="User" />.
        /// </summary>
        [JsonProperty("user")]
        public ICollection<User> Users { get; set; }
    }
}