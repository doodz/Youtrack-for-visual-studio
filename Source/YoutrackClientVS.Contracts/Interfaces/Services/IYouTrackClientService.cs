using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IYouTrackClientService
    {
        string Title { get; }
        string Origin { get; }
        Task LoginAsync(YouTrackCredentials youTrackCredentials);
        void Logout();
        Task<IEnumerable<YouTrackProject>> GetAllProjects(bool verbose = false);
        Task<GitRemoteRepository> CreateRepositoryAsync(GitRemoteRepository newRepository);
        Task<IEnumerable<GitBranch>> GetBranches();
        Task<IEnumerable<GitTeam>> GetTeams();
        Task<IEnumerable<GitCommit>> GetPullRequestCommits(long id);
        Task<IEnumerable<GitComment>> GetPullRequestComments(long id);

        Task DisapprovePullRequest(long id);
        Task<bool> ApprovePullRequest(long id);
        Task<bool> DeclinePullRequest(long id, string version);
        Task<bool> MergePullRequest(GitMergeRequest request);
        Task<IEnumerable<YouTrackUser>> GetUsers();
        bool IsOriginRepo(GitRemoteRepository gitRemoteRepository);
        Task CreatePullRequest(GitPullRequest gitPullRequest);
        Task<YouTrackIssue> GetIssue(string id);
        Task<IEnumerable<YouTrackComment>> GetComments(string issueId);
        Task<IEnumerable<YouTrackAttachment>> GetAttachments(string issueId);
        Task<IEnumerable<GitUser>> GetRepositoryUsers(string filter);
        Task<GitPullRequest> GetPullRequestForBranches(string sourceBranch, string destBranch);
        Task<GitCommit> GetCommitById(string id);
        Task<IEnumerable<GitCommit>> GetCommitsRange(GitBranch fromBranch, GitBranch toBranch);

        Task UpdatePullRequest(GitPullRequest gitPullRequest);
        Task<IEnumerable<GitUser>> GetDefaultReviewers();
        Task<string> GetFileContent(string hash, string path);

        Task<GitComment> AddPullRequestComment(long id, GitComment comment);

        Task DeletePullRequestComment(long pullRequestId, long commentId, long version);

        Task<IEnumerable<GitPullRequest>> GetPullRequests(
            int limit = 50,
            GitPullRequestStatus? state = null,
            string fromBranch = null,
            string toBranch = null,
            bool isDescSorted = true,
            string author = null
        );

        Task<IEnumerable<YouTrackIssue>> GetIssuesByProject(string projectId, int limit = 5000);
        Task<IEnumerable<YouTrackIssue>> GetIssuesPage(
           int page,
           int limit = 50,
           YouTrackStatusSearch? state = null,
           string project = null,
           string author = null
       );

        Task<GitComment> EditPullRequestComment(long id, GitComment comment);
        Uri GetIssueUri(string issueId);
    }
}
