using System;

namespace YouTrackClientVS.Contracts.Models.YouTrackClientModels
{
    /// <summary>
    /// Represents an <see cref="YouTrackAttachment" /> for an <see cref="YouTrackIssue" />.
    /// </summary>
    public class YouTrackAttachment
    {

        /// <summary>
        /// Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Url.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Group.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Created.
        /// </summary>
        public DateTime Created { get; set; }

    }
}