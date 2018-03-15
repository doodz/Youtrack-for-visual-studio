using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    /// <summary>
    /// Wrapper for <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Attachment" />.
    /// </summary>
    internal class AttachmentCollectionWrapper
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Attachment" />.
        /// </summary>
        [JsonProperty("fileUrl")]
        public ICollection<Attachment> Attachments { get; set; }
    }
}