using System;
using System.Collections.Generic;

namespace YouTrackClientVS.Contracts.Models.YouTrackClientModels
{
    /// <summary>
    /// A class that represents YouTrack issue comment information.
    /// </summary>
    public class YouTrackComment
    {
        /// <summary>
        /// Creates an instance of the <see cref="Comment" /> class.
        /// </summary>
        public YouTrackComment()
        {
            Replies = new List<YouTrackComment>();
        }

        /// <summary>
        /// Id of the comment.
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// Author of the comment.
        /// </summary>

        public string Author { get; set; }

        /// <summary>
        /// Author of the comment (full name).
        /// </summary>

        public string AuthorFullName { get; set; }

        /// <summary>
        /// Issue id to which the comment belongs.
        /// </summary>

        public string IssueId { get; set; }

        /// <summary>
        /// Parent comment id.
        /// </summary>

        public string ParentId { get; set; }

        /// <summary>
        /// Is the comment deleted?
        /// </summary>

        public bool IsDeleted { get; set; }

        /// <summary>
        /// If comment was imported from JIRA, represents the Id it has in JIRA.
        /// </summary>

        public string JiraId { get; set; }

        /// <summary>
        /// Text of the comment.
        /// </summary>

        public string Text { get; set; }

        /// <summary>
        /// Is the comment shown for the issue author?
        /// </summary>

        public bool ShownForIssueAuthor { get; set; }

        /// <summary>
        /// Represents when the issue was created.
        /// </summary>

        public DateTime? Created { get; set; }

        /// <summary>
        /// Represents when the issue was updated.
        /// </summary>

        public DateTime? Updated { get; set; }

        /// <summary>
        /// Permitted group.
        /// </summary>

        public string PermittedGroup { get; set; }

        /// <summary>
        /// Replies.
        /// </summary>

        public ICollection<YouTrackComment> Replies { get; set; }
    }
}
