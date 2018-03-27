using LibGit2Sharp;
using log4net;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.IO;
using System.Linq;
using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.TeamFoundation.Extensions
{
    public static class GitRepositoryExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static GitRemoteRepository ToGitRepo(this IGitRepositoryInfo source)
        {
            if (source == null) return null;

            var dir = new DirectoryInfo(source.RepositoryPath);
            var repoPath = Repository.Discover(source.RepositoryPath);
            var repo = repoPath == null ? null : new Repository(repoPath);
            if (repo == null) return null;

            return CreateGitRepo(repo);
        }

        private static GitRemoteRepository CreateGitRepo(Repository repo)
        {
            var repoUrl = repo.Network.Remotes["origin"]?.Url ?? repo.Network.Remotes.FirstOrDefault()?.Url;
            if (repoUrl == null) return new GitRemoteRepository();

            try
            {
                var repoUri = new Uri(repoUrl);
                var repoName = repoUri.Segments.Last().TrimEnd('/').TrimEnd(".git");
                var ownerName = (repoUri.Segments[repoUri.Segments.Length - 2] ?? "").TrimEnd('/');

                var branches = repo.Branches
                    .Select(x => new GitLocalBranch()
                    {
                        Name = x.FriendlyName,
                        IsRemote = x.IsRemote,
                        IsHead = x.IsCurrentRepositoryHead,
                        TrackedBranchName = x.TrackedBranch?.FriendlyName.Replace(x.TrackedBranch?.RemoteName + "/", string.Empty),
                        Target = new GitCommit() { Hash = x.Tip?.Sha },
                    })
                    .ToList();

                return new GitRemoteRepository(repoName, ownerName, repoUrl, branches);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}