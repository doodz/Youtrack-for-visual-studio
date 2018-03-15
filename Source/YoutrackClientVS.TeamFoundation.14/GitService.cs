using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using log4net;
using LibGit2Sharp;
using Microsoft.TeamFoundation.Git.Controls.Extensibility;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.TeamFoundation.Extensions;
using CloneOptions = Microsoft.TeamFoundation.Git.Controls.Extensibility.CloneOptions;

namespace YouTrackClientVS.TeamFoundation
{
    [Export(typeof(IGitService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class GitService : IGitService
    {
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _remoteName;
        private readonly string _mainBranch;

        private readonly IAppServiceProvider _appServiceProvider;

        /// <summary>
        /// This MEF export requires specific versions of TeamFoundation. 
        /// </summary>
        private IGitExt _gitService;


        [ImportingConstructor]
        public GitService(IAppServiceProvider appServiceProvider)
        {
            _appServiceProvider = appServiceProvider;
            _remoteName = "origin";
            _mainBranch = "master";
        }

        private Credentials CreateCredentials(string url, string user,
            SupportedCredentialTypes supportedCredentialTypes)
        {
            var userInformationService = _appServiceProvider.GetService<IUserInformationService>();
            return new SecureUsernamePasswordCredentials
            {
                Username = userInformationService.ConnectionData.UserName,
                Password = userInformationService.ConnectionData.Password.ToSecureString(),
            };
        }

        public GitRemoteRepository GetActiveRepository()
        {
            _gitService = _appServiceProvider.GetService<IGitExt>();
            var activeRepository = _gitService.ActiveRepositories.FirstOrDefault();
            return activeRepository.ToGitRepo();
        }

        public IEnumerable<LocalRepo> GetLocalRepositories()
        {
            var localRepositories = RegistryHelper
                .GetLocalRepositories()
                .Select(path =>
                    new LocalRepo(path.Split(Path.DirectorySeparatorChar).LastOrDefault(), path, GetCloneUrl(path)))
                .Where(x => x.Name != null && x.ClonePath != null)
                .ToList();

            var activeRepo = _gitService.ActiveRepositories.FirstOrDefault();

            if (activeRepo == null) return localRepositories;
            {
                var localActiveRepo = localRepositories.FirstOrDefault(x =>
                    string.Compare(x.LocalPath, activeRepo.RepositoryPath, StringComparison.OrdinalIgnoreCase) == 0);
                if (localActiveRepo != null)
                    localActiveRepo.IsActive = true;
            }

            return localRepositories;
        }

        private static string GetCloneUrl(string path)
        {
            var repoPath = Repository.Discover(path);
            var repo = repoPath == null ? null : new Repository(repoPath);
            var cloneUrl = repo.Network.Remotes["origin"]?.Url ?? repo.Network.Remotes.FirstOrDefault()?.Url;
            return cloneUrl;
        }

        public void CloneRepository(string cloneUrl, string repositoryName, string repositoryPath)
        {
            var gitExt = _appServiceProvider.GetService<IGitRepositoriesExt>();

            var path = Path.Combine(repositoryPath, repositoryName);

            Directory.CreateDirectory(path);

            try
            {
                gitExt.Clone(cloneUrl, path, CloneOptions.RecurseSubmodule);
            }
            catch (Exception ex)
            {
                Logger.Error($"Could not clone {cloneUrl} to {path}. {ex}");
                throw;
            }
        }

        private Repository GetRepository()
        {
            var gitExt = _appServiceProvider.GetService<IGitExt>();
            // _gitService = gitExt;
            var vsRepo = gitExt.ActiveRepositories.FirstOrDefault();

            Repository activeRepository;
            if (vsRepo == null)
            {
                var vsSolution = _appServiceProvider.GetService<IVsSolution>();
                string solutionDir, solutionFile, userFile;
                if (!ErrorHandler.Succeeded(vsSolution.GetSolutionInfo(out solutionDir, out solutionFile, out userFile))
                )
                {
                    Logger.Error($"Could not find active repository");
                    throw new Exception();
                }

                if (solutionDir == null)
                {
                    Logger.Error($"Could not find active repository");
                    throw new Exception();
                }

                activeRepository = new Repository(Repository.Discover(solutionDir));
            }
            else
            {
                activeRepository = new Repository(Repository.Discover(vsRepo.RepositoryPath));
            }

            return activeRepository;
        }

        public string GetDefaultRepoPath()
        {
            try
            {
                return RegistryHelper.GetLocalClonePath();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void SetRemote(Repository activeRepository, string cloneUrl)
        {
            activeRepository.Config.Set($"remote.{_remoteName}.url", cloneUrl);
            activeRepository.Config.Set($"remote.{_remoteName}.fetch", $"+refs/heads/*:refs/remotes/{_remoteName}/*");
        }

        private void Push(Repository activeRepository)
        {
            var pushOptions = new PushOptions()
            {
                CredentialsProvider = CreateCredentials
            };

            var remote = activeRepository.Network.Remotes[_remoteName];
            if (activeRepository.Head?.Commits != null && activeRepository.Head.Commits.Any()) activeRepository.Network.Push(remote, "HEAD", @"refs/heads/" + _mainBranch, pushOptions);
        }

        private void Fetch(Repository activeRepository)
        {
            var fetchOptions = new FetchOptions()
            {
                CredentialsProvider = CreateCredentials
            };
            Commands.Fetch(activeRepository, _remoteName, new string[0], fetchOptions, null);
        }

        private void SetTrackingRemote(Repository activeRepository)
        {
            var remoteBranchName = "refs/remotes/" + _remoteName + "/" + _mainBranch;
            var remoteBranch = activeRepository.Branches[remoteBranchName];
            // if it's null, it's because nothing was pushed
            if (remoteBranch != null)
            {
                var localBranchName = "refs/heads/" + _mainBranch;
                var localBranch = activeRepository.Branches[localBranchName];
                activeRepository.Branches.Update(localBranch, b => b.TrackedBranch = remoteBranch.CanonicalName);
            }
        }

        public void PublishRepository(GitRemoteRepository repository)
        {
            var activeRepository = GetRepository();
            SetRemote(activeRepository, repository.CloneUrl);
            Push(activeRepository);
            Fetch(activeRepository);
            SetTrackingRemote(activeRepository);
        }
    }
}