using log4net;
//using ParseDiff;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using YouTrack.REST.API;
using YouTrack.REST.API.Authentication;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.Services
{
    [Export(typeof(IYouTrackClientService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class YouTrackService : IYouTrackClientService
    {
        private readonly IEventAggregatorService _eventAggregator;
        private readonly IGitWatcher _gitWatcher;
        private IYouTrackClient _youTrackClient;

        private readonly ILog _logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsConnected => _youTrackClient != null;

        [ImportingConstructor]
        public YouTrackService(IEventAggregatorService eventAggregator, IGitWatcher gitWatcher)
        {
            _eventAggregator = eventAggregator;
            _gitWatcher = gitWatcher;
        }


        public string Origin => "YouTrack";
        public string Title => $"{Origin} Extension";
        private readonly string _supportedScm = "git";

        public async Task LoginAsync(YouTrackCredentials youTrackCredentials)
        {
            if (IsConnected)
                return;

            OnConnectionChanged(new ConnectionData() { IsLoggingIn = true });

            if (string.IsNullOrEmpty(youTrackCredentials.Login) ||
                string.IsNullOrEmpty(youTrackCredentials.Password))
                throw new Exception("Credentials fields cannot be empty");

            var connectionData = ConnectionData.NotLogged;

            try
            {
                connectionData = new ConnectionData()
                {
                    Password = youTrackCredentials.Password,
                    Host = youTrackCredentials.Host,
                    IsLoggingIn = false
                };
                _youTrackClient = await CreateYouTrackClientAsync(youTrackCredentials);

                connectionData.UserName = _youTrackClient.ApiConnection.Credentials.Login;
                connectionData.IsLoggedIn = true;
            }
            finally
            {
                OnConnectionChanged(connectionData);
            }
        }

        private async Task<IYouTrackClient> CreateYouTrackClientAsync(YouTrackCredentials youTrackCredentials)
        {
            _logger.Info($"Calling CreateYouTrackClient. Host: {youTrackCredentials.Host}");
            var credentials = new Credentials(youTrackCredentials.Login, youTrackCredentials.Password);

            var apiUrl = youTrackCredentials.Host.AbsoluteUri;
            if (!apiUrl.EndsWith("/"))
                apiUrl += "/rest";
            else
                apiUrl += "rest";

            var apiConnection = new Connection(youTrackCredentials.Host, new Uri(apiUrl), credentials);
            var client = new EnterpriseYouTrackClient(apiConnection);
            var user = await client.UserClient.Login(); //will throw exception if not authenticated
            // await client.UserClient.GetCurrentUserInfo();
            return client;
        }


        public async Task<IEnumerable<GitUser>> GetRepositoryUsers(string filter)
        {
            return (await _youTrackClient.PullRequestsClient
                    .GetRepositoryUsers(_gitWatcher.ActiveRepo.Name, _gitWatcher.ActiveRepo.Owner, filter))
                .MapTo<List<GitUser>>();
        }

        public async Task<IEnumerable<YouTrackProject>> GetAllProjects(bool verbose = false)
        {
            var projects = await _youTrackClient.ProjectsClient.GetAccessibleProjects(verbose);
            return projects.MapTo<List<YouTrackProject>>();
        }

        //public async Task<IEnumerable<YouTrackProject>> GetGroups(string username)
        //{
        //    var projects = await _youTrackClient.UserManagementClient.GetGroupsForUser(username);
        //    return projects.MapTo<List<youtr>>();
        //}

        public async Task<IEnumerable<GitTeam>> GetTeams()
        {
            var teams = await _youTrackClient.TeamsClient.GetTeams();
            return teams.MapTo<List<GitTeam>>();
        }

        public async Task<YouTrackIssue> GetIssue(string id)
        {
            return (await _youTrackClient.IssuesClient.GetIssue(id)).MapTo<YouTrackIssue>();
        }

        public async Task CreateIssue(string projectId, YouTrackIssue youTrackIssue)
        {
            await _youTrackClient.IssuesClient.CreateIssue(projectId, youTrackIssue.MapTo<Issue>());
        }

        public async Task<IEnumerable<YouTrackUser>> GetUsers()
        {
            return (await _youTrackClient.UserManagementClient.GetUsers()).MapTo<List<YouTrackUser>>();
        }

        public async Task<YouTrackIntellisense> GetIntellisense(string project, string filter)
        {
            return (await _youTrackClient.IssuesClient.GetIntellisense(project, filter)).MapTo<YouTrackIntellisense>();
        }

        public async Task<IEnumerable<YouTrackComment>> GetComments(string issueId)
        {
            return (await _youTrackClient.IssuesClient.GetCommentsForIssue(issueId)).MapTo<List<YouTrackComment>>();
        }

        public async Task<IEnumerable<YouTrackAttachment>> GetAttachments(string issueId)
        {
            return (await _youTrackClient.IssuesClient.GetAttachmentsForIssue(issueId))
                .MapTo<List<YouTrackAttachment>>();
        }

        public async Task<string> GetFileContent(string hash, string path)
        {
            return await _youTrackClient.PullRequestsClient.GetFileContent(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, hash, path);
        }

        public async Task<GitComment> EditPullRequestComment(long id, GitComment comment)
        {
            var response = await _youTrackClient.PullRequestsClient.EditPullRequestComment(
                _gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner,
                id,
                comment.Id,
                comment.Content.Html,
                comment.Version
            );

            return response.MapTo<GitComment>();
        }

        public async Task<GitComment> AddPullRequestComment(long id, GitComment comment)
        {
            var response = await _youTrackClient.PullRequestsClient.AddPullRequestComment(
                _gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner,
                id,
                comment.Content.Html,
                comment.Inline?.From,
                comment.Inline?.To,
                comment.Inline?.Path,
                comment.Parent?.Id);

            return response.MapTo<GitComment>();
        }

        public async Task DeletePullRequestComment(long pullRequestId, long commentId, long version)
        {
            await _youTrackClient.PullRequestsClient.DeletePullRequestComment(
                _gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner,
                pullRequestId,
                commentId,
                version
            );
        }

        public bool IsOriginRepo(GitRemoteRepository gitRemoteRepository)
        {
            if (gitRemoteRepository?.CloneUrl == null) return false;
            var uri = new Uri(gitRemoteRepository.CloneUrl);
            return _youTrackClient.ApiConnection.ApiUrl.Host.Contains(uri.Host, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<GitRemoteRepository> CreateRepositoryAsync(GitRemoteRepository newRepository)
        {
            var repository = newRepository.MapTo<Repository>();
            repository.Name = repository.Name.Replace(' ', '-');
            var result = await _youTrackClient.RepositoriesClient.CreateRepository(repository);
            return result.MapTo<GitRemoteRepository>();
        }

        public async Task<GitPullRequest> GetPullRequestForBranches(string sourceBranch, string destBranch)
        {
            var pullRequest = await _youTrackClient.PullRequestsClient
                .GetPullRequestForBranches(_gitWatcher.ActiveRepo.Name, _gitWatcher.ActiveRepo.Owner, sourceBranch,
                    destBranch);
            return pullRequest?.MapTo<GitPullRequest>();
        }


        public async Task<IEnumerable<GitCommit>> GetCommitsRange(GitBranch fromBranch, GitBranch toBranch)
        {
            var from = new Branch()
            {
                Name = fromBranch.Name,
                Target = new Commit() { Hash = fromBranch.Target.Hash }
            };

            var to = new Branch()
            {
                Name = toBranch.Name,
                Target = new Commit() { Hash = toBranch.Target.Hash }
            };

            var commits = await _youTrackClient.RepositoriesClient.GetCommitsRange(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, from, to);
            return commits.MapTo<List<GitCommit>>();
        }

        public async Task<IEnumerable<GitPullRequest>> GetPullRequests(
            int limit = 50,
            GitPullRequestStatus? state = null,
            string fromBranch = null,
            string toBranch = null,
            bool isDescSorted = true,
            string author = null
        )
        {
            var builder = _youTrackClient.PullRequestsClient.GetPullRequestQueryBuilder()
                .WithState(state?.ToString() ?? "ALL")
                .WithOrder(isDescSorted ? Order.Newest : Order.Oldest)
                .WithSourceBranch(fromBranch)
                .WithDestinationBranch(toBranch)
                .WithAuthor(author, null);

            return (await _youTrackClient.PullRequestsClient
                    .GetPullRequests(_gitWatcher.ActiveRepo.Name, _gitWatcher.ActiveRepo.Owner, limit, builder))
                .MapTo<List<GitPullRequest>>();
        }

        public async Task<IEnumerable<YouTrackIssue>> GetIssuesByProject(string projectId, int limit = 5000)
        {
            var builder = _youTrackClient.IssuesClient.GetIssueQueryBuilder()
                .After(0)
                .Max(limit)
                .WithWikifyDescription(false);
            builder.AddFilter("State:unresolved");

            return (await _youTrackClient.IssuesClient.GetIssuesByProject(projectId, builder))
                .MapTo<List<YouTrackIssue>>();
        }

        public async Task<IEnumerable<YouTrackIssue>> GetIssuesPage(
            int page,
            int limit = 50,
            YouTrackStatusSearch? state = null,
            string project = null,
            string filter = null
        )
        {
            var builder = _youTrackClient.IssuesClient.GetIssueQueryBuilder()
                .After(page * limit)
                .Max(limit)
                .WithWikifyDescription(false);

            if (state != null)
                switch (state)
                {
                    case YouTrackStatusSearch.InProgress:
                        builder.AddFilter("State:In progress");
                        break;
                    case YouTrackStatusSearch.ToBeDiscussed:
                        builder.AddFilter("State:To be discussed");
                        break;
                    default:
                        builder.AddFilter(string.Concat("State:", state.ToString()));
                        break;
                }

            if (filter != null) builder.AddFilter(filter);
            //if (author != null)
            //{
            //    builder.AddFilter($"Created:{author}");
            //}

            if (project != null)
                return (await _youTrackClient.IssuesClient.GetIssuesByProject(project, builder))
                    .MapTo<List<YouTrackIssue>>();
            else
                return (await _youTrackClient.IssuesClient.GetListIssues(builder)).MapTo<List<YouTrackIssue>>();
        }

        public async Task<IEnumerable<GitBranch>> GetBranches()
        {
            var repositories =
                await _youTrackClient.RepositoriesClient.GetBranches(_gitWatcher.ActiveRepo.Name,
                    _gitWatcher.ActiveRepo.Owner);
            return repositories.MapTo<List<GitBranch>>();
        }

        public async Task<GitCommit> GetCommitById(string id)
        {
            var commit = await _youTrackClient.RepositoriesClient.GetCommitById(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, id);
            return commit.MapTo<GitCommit>();
        }


        public async Task<bool> ApprovePullRequest(long id)
        {
            var result = await _youTrackClient.PullRequestsClient.ApprovePullRequest(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, id);
            return result != null && result.Approved;
        }

        public async Task<bool> DeclinePullRequest(long id, string version)
        {
            await _youTrackClient.PullRequestsClient.DeclinePullRequest(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, id, version);
            return true;
        }

        public async Task<bool> MergePullRequest(GitMergeRequest request)
        {
            var req = request.MapTo<MergeRequest>();
            await _youTrackClient.PullRequestsClient.MergePullRequest(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, req);
            return true;
        }

        public async Task DisapprovePullRequest(long id)
        {
            await _youTrackClient.PullRequestsClient.DisapprovePullRequest(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, id);
        }



        public async Task UpdatePullRequest(GitPullRequest gitPullRequest)
        {
            await _youTrackClient.PullRequestsClient
                .UpdatePullRequest(gitPullRequest.MapTo<PullRequest>(), _gitWatcher.ActiveRepo.Name,
                    _gitWatcher.ActiveRepo.Owner);
        }

        public async Task<IEnumerable<GitUser>> GetDefaultReviewers()
        {
            return (await _youTrackClient.PullRequestsClient
                    .GetDefaultReviewers(_gitWatcher.ActiveRepo.Name, _gitWatcher.ActiveRepo.Owner))
                .MapTo<List<GitUser>>();
        }

        public void Logout()
        {
            _youTrackClient = null;
            OnConnectionChanged(ConnectionData.NotLogged);
        }

        private void OnConnectionChanged(ConnectionData connectionData)
        {
            _eventAggregator.Publish(new ConnectionChangedEvent(connectionData));
        }

        public async Task<IEnumerable<GitCommit>> GetPullRequestCommits(long id)
        {
            var commits = await _youTrackClient.PullRequestsClient.GetPullRequestCommits(_gitWatcher.ActiveRepo.Name,
                _gitWatcher.ActiveRepo.Owner, id);
            return commits.MapTo<List<GitCommit>>();
        }

        public async Task<IEnumerable<GitComment>> GetPullRequestComments(long id)
        {
            throw new NotImplementedException();
        }

        public Uri GetIssueUri(string issueId)
        {
            return _youTrackClient.ApiConnection.MainUrl.Combine($"issue/{issueId}");
        }

    }
}