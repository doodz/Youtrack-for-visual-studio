using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.TeamFoundation.Extensions;

namespace YouTrackClientVS.TeamFoundation
{
    [Export(typeof(IGitWatcher))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class GitWatcher : IGitWatcher
    {
        private readonly SynchronizationContext _syncContext;
        private readonly IGitExt _gitExt;
        private readonly IEventAggregatorService _eventAggregatorService;
        private readonly IGitService _gitService;

        [ImportingConstructor]
        public GitWatcher(
            IAppServiceProvider appServiceProvider,
            IEventAggregatorService eventAggregatorService,
            IGitService gitService
        )
        {
            _syncContext = SynchronizationContext.Current;
            this._eventAggregatorService = eventAggregatorService;
            _gitService = gitService;
            _gitExt = appServiceProvider.GetService<IGitExt>();
        }

        public void Initialize()
        {
            ActiveRepo = _gitService.GetActiveRepository();
            _gitExt.PropertyChanged += CheckAndUpdate;
        }

        public void Refresh()
        {
            ActiveRepo = _gitService.GetActiveRepository();
        }

        void CheckAndUpdate(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IGitExt.ActiveRepositories))
                return;

            var repo = _gitExt.ActiveRepositories.FirstOrDefault()?.ToGitRepo();
            if (repo != ActiveRepo)
                _syncContext.Post(r => ActiveRepo = r as GitRemoteRepository, repo);
        }


        private GitRemoteRepository _activeRepo;

        public GitRemoteRepository ActiveRepo
        {
            get => _activeRepo;
            private set
            {
                var previousRepo = _activeRepo;
                _activeRepo = value;
                _eventAggregatorService.Publish(new ActiveRepositoryChangedEvent(value, previousRepo));
            }
        }
    }
}