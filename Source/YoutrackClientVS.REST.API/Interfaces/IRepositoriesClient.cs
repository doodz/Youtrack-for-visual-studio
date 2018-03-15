using System.Collections.Generic;
using System.Threading.Tasks;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Interfaces
{
    public interface IRepositoriesClient
    {
        Task<IEnumerable<Repository>> GetUserRepositories();
        Task<IEnumerable<Branch>> GetBranches(string repoName, string owner);
        Task<Repository> CreateRepository(Repository repository);
        Task<Commit> GetCommitById(string repoName, string owner, string id);
        Task<IEnumerable<Commit>> GetCommitsRange(string repoName, string owner, Branch fromBranch, Branch toBranch);
    }
}