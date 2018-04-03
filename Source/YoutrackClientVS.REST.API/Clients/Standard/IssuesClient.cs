using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using YouTrack.REST.API.Helpers;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.QueryBuilders;
using YouTrack.REST.API.Wrappers;

namespace YouTrack.REST.API.Clients.Standard
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html">YouTrack Issues Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class IssuesClient : ApiClient, IIssuesClient
    {
        private static readonly string[] ReservedFields =
        {
            "id", "entityid", "jiraid", "summary", "description"
        };

        /// <summary>
        /// Creates an instance of the <see cref="IssuesService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="System.Net.Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public IssuesClient(IEnterpriseYouTrackRestClient restClient, Connection connection) : base(restClient,
            connection)
        {
        }

        /// <summary>
        /// Get a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-an-Issue.html">Get an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to get.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>The <see cref="Issue" /> that matches the requested <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Issue> GetIssue(string issueId, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            var url = IssuesUrls.GetIssue(issueId, wikifyDescription);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync<Issue>(request);

            return response.Data;
        }

        /// <summary>
        /// Get all issues by project
        /// </summary>
        ///<remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Issues-in-a-Project.html">Get Issues in a Project</a>.</remarks>
        /// <param name="projectId">ProjectID of a project to get issues from.</param>
        /// <returns>The <see cref="Issue" /> list that matches the requested <paramref name="projectId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Issue>> GetIssuesByProject(string projectId, IIssueQueryBuilder builder = null)
        {
            if (string.IsNullOrEmpty(projectId)) throw new ArgumentNullException(nameof(projectId));

            var url = IssuesUrls.GetIssuesByProject(projectId);
            var request = new YouTrackRestRequest(url, Method.GET);

            if (builder != null)
                foreach (var param in builder.GetQueryParameters())
                    request.AddQueryParameter(param.Key, param.Value);

            var response = await RestClient.ExecuteTaskAsync<List<Issue>>(request);

            return response.Data;
        }

        /// <summary>
        /// Get a list of issues that match one or more search queries.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-the-List-of-Issues.html">Get Issues</a>.</remarks>
        /// <returns>The <see cref="Issue" /> list that matches the requested</returns>
        public async Task<IEnumerable<Issue>> GetListIssues(IIssueQueryBuilder builder = null)
        {
            var url = IssuesUrls.GetIssues();
            var request = new YouTrackRestRequest(url, Method.GET);

            if (builder != null)
                foreach (var param in builder.GetQueryParameters())
                    request.AddQueryParameter(param.Key, param.Value);


            var response = await RestClient.ExecuteTaskAsync<IssueCollectionWrapper>(request);

            return response.Data.Issues;
        }

        /// <summary>
        /// Checks whether an issue exists on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Check-that-an-Issue-Exists.html">Check that an Issue Exists</a>.</remarks>
        /// <param name="issueId">Id of an issue to check.</param>
        /// <returns><value>True</value> if the issue exists, otherwise <value>false</value>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<bool> Exists(string issueId)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            var url = IssuesUrls.Exists(issueId);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync(request);

            return response.StatusCode != HttpStatusCode.NotFound;
        }

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
        public async Task<string> CreateIssue(string projectId, Issue issue)
        {
            if (string.IsNullOrEmpty(projectId)) throw new ArgumentNullException(nameof(projectId));

            //TODO THE queryBuilder ?
            var url = IssuesUrls.CreateIssue();
            var request = new YouTrackRestRequest(url, Method.PUT);
            request.AddQueryParameter("project", projectId);

            if (!string.IsNullOrEmpty(issue.Summary)) request.AddQueryParameter("summary", WebUtility.UrlEncode(issue.Summary));

            if (!string.IsNullOrEmpty(issue.Description)) request.AddQueryParameter("description", WebUtility.UrlEncode(issue.Summary));


            request.AddHeader("Content-Type", "multipart/form-data");
            var response = await RestClient.ExecuteTaskAsync(request);


            // Extract issue id from Location header response
            var marker = "rest/issue/";


            var locationHeader =
                response.Headers.FirstOrDefault(h => h.Name.Equals("Location", StringComparison.OrdinalIgnoreCase));

            var issueId =
                locationHeader.Value.ToString().Substring(
                    locationHeader.Value.ToString().IndexOf(marker, StringComparison.OrdinalIgnoreCase) +
                    marker.Length);

            // For every custom field, apply a command
            var customFields = issue.Fields
                .Where(field => !ReservedFields.Contains(field.Name.ToLower()))
                .ToDictionary(field => field.Name, field => field.Value);

            foreach (var customField in customFields)
                if (!(customField.Value is string) && customField.Value is System.Collections.IEnumerable enumerable)
                    await ApplyCommand(issueId, $"{customField.Key} {string.Join(" ", enumerable.OfType<string>())}",
                        string.Empty);
                else if (customField.Value is DateTime dateTime)
                    await ApplyCommand(issueId, $"{customField.Key} {dateTime:s}", string.Empty);
                else if (customField.Value is DateTimeOffset dateTimeOffset)
                    await ApplyCommand(issueId, $"{customField.Key} {dateTimeOffset:s}", string.Empty);
                else
                    await ApplyCommand(issueId, $"{customField.Key} {customField.Value}", string.Empty);

            // Add comments?
            foreach (var issueComment in issue.Comments) await ApplyCommand(issueId, "comment", issueComment.Text, runAs: issueComment.Author);

            // Add tags?
            foreach (var issueTag in issue.Tags) await ApplyCommand(issueId, $"tag {issueTag.Value}");

            return issueId;
        }

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
        public async Task UpdateIssue(string issueId, string summary = null, string description = null)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            if (summary == null && description == null) return;

            var url = IssuesUrls.UpdateIssue(issueId);
            var request = new YouTrackRestRequest(url, Method.POST);

            if (!string.IsNullOrEmpty(summary)) request.AddQueryParameter("summary", WebUtility.UrlEncode(summary));

            if (!string.IsNullOrEmpty(description)) request.AddQueryParameter("description", WebUtility.UrlEncode(description));

            request.AddHeader("Content-Type", "multipart/form-data");
            var response = await RestClient.ExecuteTaskAsync(request);
        }

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
        public async Task ApplyCommand(string issueId, string command, string comment = null,
            bool disableNotifications = false, string runAs = null)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command));

            var url = IssuesUrls.ApplyCommand(issueId);
            var request = new YouTrackRestRequest(url, Method.POST);


            request.AddQueryParameter("command", WebUtility.UrlEncode(command));

            if (!string.IsNullOrEmpty(comment)) request.AddQueryParameter("comment", WebUtility.UrlEncode(comment));

            if (disableNotifications) request.AddQueryParameter("disableNotifications", WebUtility.UrlEncode("true"));

            if (!string.IsNullOrEmpty(runAs)) request.AddQueryParameter("runAs", WebUtility.UrlEncode(runAs));


            request.AddHeader("Content-Type", "multipart/form-data");
            var response = await RestClient.ExecuteTaskAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                //TODO THE implement that.
                // Try reading the error message
                //var responseJson = response.
                //if (responseJson["value"] != null)
                //{
                //    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                //}
                //else
                //{
                //    throw new YouTrackErrorException(Strings.Exception_UnknownError);
                //}
            }
        }

        /// <summary>
        /// Deletes an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Delete-an-Issue.html">Delete an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));


            var url = IssuesUrls.DeleteIssue(issueId);
            var request = new YouTrackRestRequest(url, Method.DELETE);
            var response = await RestClient.ExecuteTaskAsync(request);


            if (response.StatusCode == HttpStatusCode.NotFound) return;
        }

        /// <summary>
        /// Get links for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Links-of-an-Issue.html">Get Links of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get links for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Link" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Link>> GetLinksForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            var url = IssuesUrls.GetLinksForIssue(issueId);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync<List<Link>>(request);

            return response.Data;
        }

        /// <summary>
        /// Get comments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Comments-of-an-Issue.html">Get Comments of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then comments in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Comment>> GetCommentsForIssue(string issueId, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            var url = IssuesUrls.GetCommentsForIssue(issueId);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync<List<Comment>>(request);
            return response.Data;
        }


        /// <summary>
        /// Updates a comment for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Update-a-Comment.html">Update a Comment</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment to update.</param>
        /// <param name="text">The new text of the comment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="commentId"/> or <paramref name="text"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task UpdateCommentForIssue(string issueId, string commentId, string text)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            if (string.IsNullOrEmpty(commentId)) throw new ArgumentNullException(nameof(commentId));

            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            var myText = new SimpleText(text);
            var url = IssuesUrls.UpdateCommentForIssue(issueId, commentId);
            var request = new YouTrackRestRequest(url, Method.PUT);
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(myText),
                ParameterType.RequestBody);
            var response = await RestClient.ExecuteTaskAsync<PullRequest>(request);
        }

        /// <summary>
        /// Deletes a comment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Remove-a-Comment-for-an-Issue.html">Remove a Comment for an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment to delete.</param>
        /// <param name="permanent">When <value>true</value>, the specified comment will be deleted permanently and can not be restored afterwards. Defaults to <value>false</value>.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="commentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteCommentForIssue(string issueId, string commentId, bool permanent = false)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            if (string.IsNullOrEmpty(commentId)) throw new ArgumentNullException(nameof(commentId));

            var url = IssuesUrls.DeleteCommentForIssue(issueId, commentId, permanent);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync<List<Comment>>(request);

            if (response.StatusCode == HttpStatusCode.NotFound) return;
        }


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
        public async Task AttachFileToIssue(string issueId, IAttachFileQueryBuilder builder, Stream attachmentStream)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));

            if (attachmentStream == null) throw new ArgumentNullException(nameof(attachmentStream));

            var url = IssuesUrls.AttachFileToIssue(issueId);
            var request = new YouTrackRestRequest(url, Method.POST);

            if (builder != null)
                foreach (var param in builder.GetQueryParameters())
                    request.AddQueryParameter(param.Key, param.Value);

            request.AddHeader("Content-Type", "multipart/form-data");


            var streamContent = new StreamContent(attachmentStream);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = builder.GetAttachmentName(),
                Name = builder.GetAttachmentName()
            };

            var content = new MultipartFormDataContent
            {
                streamContent
            };

            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(content),
                ParameterType.RequestBody);
            var response = await RestClient.ExecuteTaskAsync(request);
            //var response = await client.PostAsync($"rest/issue/{issueId}/attachment?{query}", content);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                //TODO THE
                // Try reading the error message
                //var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                //if (responseJson["value"] != null)
                //{
                //    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                //}
                //else
                //{
                //    throw new YouTrackErrorException(Strings.Exception_UnknownError);
                //}
            }
        }

        /// <summary>
        /// Get attachments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Users-of-an-Issue.html">Get Users of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Attachment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));


            var url = IssuesUrls.GetAttachmentsForIssue(issueId);
            var request = new YouTrackRestRequest(url, Method.GET);

            var response = await RestClient.ExecuteTaskAsync<AttachmentCollectionWrapper>(request);

            return response.Data.Attachments;
        }

        /// <summary>
        /// Downloads an attachment from the server.
        /// </summary>
        /// <param name="attachmentUrl">The <see cref="T:System.Uri" /> of the attachment.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> containing the attachment data.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="attachmentUrl"/> is null.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Stream> DownloadAttachment(Uri attachmentUrl)
        {
            if (attachmentUrl == null) throw new ArgumentNullException(nameof(attachmentUrl));


            var request = new YouTrackRestRequest(attachmentUrl.ToString(), Method.GET);
            var response = await RestClient.ExecuteTaskAsync<Stream>(request);

            return response.Data;
        }

        /// <summary>
        /// Deletes an attachment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Delete-Attachment-from-an-Issue.html">Delete Attachment from an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the attachment belongs.</param>
        /// <param name="attachmentId">Id of the attachment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="attachmentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteAttachmentForIssue(string issueId, string attachmentId)
        {
            if (string.IsNullOrEmpty(issueId)) throw new ArgumentNullException(nameof(issueId));


            var url = IssuesUrls.DeleteAttachmentForIssue(issueId, attachmentId);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound) return;
        }


        /// <summary>
        /// Get highlight and suggestions for issue filter query.
        /// </summary>
        /// <param name="project">Short name of the context project.</param>
        ///  /// <param name="filter">Current issue search query.</param>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Intellisense-for-issue-search.html">Intellisense for issue search</a>.</remarks>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Intellisense> GetIntellisense(string project, string filter)
        {
            var url = IssuesUrls.GetIntellisense();
            //parameters are always encoded (both the name and the value)
            //https://github.com/restsharp/RestSharp/blob/master/RestSharp/RestClient.cs#L294
            //https://github.com/restsharp/RestSharp/issues/892
            var parameters = "?";
            if (project != null)
            {
                parameters += $"project={HttpUtility.UrlPathEncode(project)}";
            }

            if (filter != null)
            {
                if (project != null)
                    parameters += "&";
                parameters += $"filter={HttpUtility.UrlPathEncode(filter)}";
            }

            url = url + parameters;
            var request = new YouTrackRestRequest(url, Method.GET);
            //parameters are always encoded (both the name and the value)
            //https://github.com/restsharp/RestSharp/blob/master/RestSharp/RestClient.cs#L294
            //https://github.com/restsharp/RestSharp/issues/892
            //if (project != null)
            //{
            //    var test1 = HttpUtility.UrlPathEncode(project);
            //    var test2 = WebUtility.UrlEncode(project);
            //    request.AddParameter("project", project);
            //}

            //if (filter != null)
            //{
            //    var test1 = HttpUtility.UrlPathEncode(filter);
            //    var test2 = WebUtility.UrlEncode(filter);
            //    request.AddParameter("filter", test1);
            //}

            
             var response = await RestClient.ExecuteTaskAsync<Intellisense>(request);
            return response.Data;
        }


        public IIssueQueryBuilder GetIssueQueryBuilder()
        {
            return new IssueQueryBuilder();
        }
    }


    internal class SimpleText
    {
        [JsonProperty(PropertyName = "text")] public string Text { get; set; }

        public SimpleText(string text)
        {
            Text = text;
        }
    }
}