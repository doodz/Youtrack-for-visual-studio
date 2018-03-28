using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Interfaces
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html">YouTrack Issues Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public interface IIssuesClient
    {
        /// <summary>
        /// Get a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-an-Issue.html">Get an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to get.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>The <see cref="Issue" /> that matches the requested <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<Issue> GetIssue(string issueId, bool wikifyDescription = false);

        /// <summary>
        /// Get all issues by project
        /// </summary>
        ///<remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Issues-in-a-Project.html">Get Issues in a Project</a>.</remarks>
        /// <param name="projectId">ProjectID of a project to get issues from.</param>
        /// <returns>The <see cref="Issue" /> list that matches the requested <paramref name="projectId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Issue>> GetIssuesByProject(string projectId, IIssueQueryBuilder builder = null);


        /// <summary>
        /// Get a list of issues that match one or more search queries.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-the-List-of-Issues.html">Get Issues</a>.</remarks>
        /// <returns>The <see cref="Issue" /> list that matches the requested</returns>
        Task<IEnumerable<Issue>> GetListIssues(IIssueQueryBuilder builder = null);
        /// <summary>
        /// Checks whether an issue exists on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Check-that-an-Issue-Exists.html">Check that an Issue Exists</a>.</remarks>
        /// <param name="issueId">Id of an issue to check.</param>
        /// <returns><value>True</value> if the issue exists, otherwise <value>false</value>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<bool> Exists(string issueId);

        /// <summary>
        /// Creates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Create-New-Issue.html">Create New Issue</a>.</remarks>
        /// <param name="projectId">Id of the project to create an issue in.</param>
        /// <param name="issue">The <see cref="Issue" /> to create. At the minimum needs the Summary field populated.</param>
        /// <returns>The newly created <see cref="Issue" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<string> CreateIssue(string projectId, Issue issue);

        /// <summary>
        /// Updates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Update-an-Issue.html">Update an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to update.</param>
        /// <param name="summary">Updated summary of the issue.</param>
        /// <param name="description">Updated description of the issue.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateIssue(string issueId, string summary = null, string description = null);

        /// <summary>
        /// Applies a command to a specific issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Apply-Command-to-an-Issue.html">Apply Command to an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to apply the command to.</param>
        /// <param name="command">The command to apply. A command might contain a string of attributes and their values - you can change multiple fields with one complex command.</param>
        /// <param name="comment">A comment to add to an issue.</param>
        /// <param name="disableNotifications">When <value>true</value>, no notifications about changes made with the specified command will be sent. Defaults to <value>false</value>.</param>
        /// <param name="runAs">Login name for a user on whose behalf the command should be applied.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="command"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task ApplyCommand(string issueId, string command, string comment = null, bool disableNotifications = false,
            string runAs = null);

        /// <summary>
        /// Deletes an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Delete-an-Issue.html">Delete an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteIssue(string issueId);

        /// <summary>
        /// Get links for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Links-of-an-Issue.html">Get Links of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get links for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Link" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Link>> GetLinksForIssue(string issueId);



        /// <summary>
        /// Get comments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Comments-of-an-Issue.html">Get Comments of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then comments in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Comment>> GetCommentsForIssue(string issueId, bool wikifyDescription = false);


        /// <summary>
        /// Updates a comment for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Update-a-Comment.html">Update a Comment</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment to update.</param>
        /// <param name="text">The new text of the comment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="commentId"/> or <paramref name="text"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateCommentForIssue(string issueId, string commentId, string text);

        /// <summary>
        /// Deletes a comment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Remove-a-Comment-for-an-Issue.html">Remove a Comment for an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment to delete.</param>
        /// <param name="permanent">When <value>true</value>, the specified comment will be deleted permanently and can not be restored afterwards. Defaults to <value>false</value>.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="commentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteCommentForIssue(string issueId, string commentId, bool permanent = false);


        /// <summary>
        /// Attaches a file to an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Attach-File-to-an-Issue.html">Attach File to an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to attach the file to.</param>
        /// <param name="attachmentName">Filename for the attachment.</param>
        /// <param name="attachmentStream">The <see cref="T:System.IO.Stream"/> to attach.</param>
        /// <param name="group">Attachment visibility group.</param>
        /// <param name="author">Creator of the attachment. Note to define author the 'Low-Level Administration' permission is required.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="attachmentName"/> or <paramref name="attachmentStream"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task AttachFileToIssue(string issueId, IAttachFileQueryBuilder builder, Stream attachmentStream);

        /// <summary>
        /// Get attachments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Users-of-an-Issue.html">Get Users of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Attachment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string issueId);

        /// <summary>
        /// Downloads an attachment from the server.
        /// </summary>
        /// <param name="attachmentUrl">The <see cref="T:System.Uri" /> of the attachment.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> containing the attachment data.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="attachmentUrl"/> is null.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<Stream> DownloadAttachment(Uri attachmentUrl);

        /// <summary>
        /// Deletes an attachment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Delete-Attachment-from-an-Issue.html">Delete Attachment from an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the attachment belongs.</param>
        /// <param name="attachmentId">Id of the attachment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="attachmentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteAttachmentForIssue(string issueId, string attachmentId);

        /// <summary>
        /// Get highlight and suggestions for issue filter query.
        /// </summary>
        /// <param name="project">Short name of the context project.</param>
        ///  /// <param name="filter">Current issue search query.</param>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Intellisense-for-issue-search.html">Intellisense for issue search</a>.</remarks>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<Intellisense> GetIntellisense(string project, string filter);

        IIssueQueryBuilder GetIssueQueryBuilder();
    }
}