using System.Collections.Generic;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IGitService
    {
        GitRemoteRepository GetActiveRepository();
        void CloneRepository(string cloneUrl, string repositoryName, string repositoryPath);
        void PublishRepository(GitRemoteRepository repository);
        string GetDefaultRepoPath();
        IEnumerable<LocalRepo> GetLocalRepositories();
    }
}