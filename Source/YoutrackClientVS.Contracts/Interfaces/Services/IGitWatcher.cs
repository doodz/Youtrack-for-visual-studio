using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IGitWatcher
    {
        GitRemoteRepository ActiveRepo { get; }
        void Initialize();
        void Refresh();
    }
}