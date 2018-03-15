using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Contracts.Events
{
    public class ActiveRepositoryChangedEvent
    {
        public GitRemoteRepository ActiveRepository { get; set; }
        public GitRemoteRepository PreviousRepository { get; set; }

        public ActiveRepositoryChangedEvent(GitRemoteRepository activeRepository, GitRemoteRepository previousRepository)
        {
            PreviousRepository = previousRepository;
            ActiveRepository = activeRepository;
        }

        public bool IsRepositoryDifferent => ActiveRepository?.Name != PreviousRepository?.Name;
    }
}