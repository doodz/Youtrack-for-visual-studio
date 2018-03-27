using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IPendingChangesSectionViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PendingChangesSectionViewModel : ViewModelBase, IPendingChangesSectionViewModel
    {
        private ReactiveCommand _initializeCommand;
        private ReactiveCommand _updateTicketCommand;
        private ReactiveCommand _updateCommitCommand;
        private string _errorMessage;
        private bool _isLoading;
        private readonly IUserInformationService _userInformationService;
        private readonly IYouTrackClientService _youTrackClientService;
        private readonly IAppServiceProvider _appServiceProvider;
        private ConnectionData _connectionData;
        private List<YouTrackProject> _localProjects;
        private YouTrackProject _selectProject;
        private List<YouTrackIssue> _localIssues;
        private YouTrackIssue _selectIssue;

        public ConnectionData ConnectionData
        {
            get => _connectionData;
            set => this.RaiseAndSetIfChanged(ref _connectionData, value);
        }

        public ICommand InitializeCommand => _initializeCommand;
        public ICommand UpdateTicketCommand => _updateTicketCommand;
        public ICommand UpdateCommitCommand => _updateCommitCommand;

        private readonly ILog _logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        public List<YouTrackProject> LocalProjects
        {
            get => _localProjects;
            set => this.RaiseAndSetIfChanged(ref _localProjects, value);
        }

        public YouTrackProject SelectedProject
        {
            get => _selectProject;
            set => this.RaiseAndSetIfChanged(ref _selectProject, value);
        }

        public List<YouTrackIssue> LocalIssues
        {
            get => _localIssues;
            set => this.RaiseAndSetIfChanged(ref _localIssues, value);
        }

        public YouTrackIssue SelectedIssue
        {
            get => _selectIssue;
            set => this.RaiseAndSetIfChanged(ref _selectIssue, value);
        }

        [ImportingConstructor]
        public PendingChangesSectionViewModel(IUserInformationService userInformationService,
            IYouTrackClientService youTrackClientService,
            IAppServiceProvider appServiceProvider
               )
        {

            _youTrackClientService = youTrackClientService;
            _userInformationService = userInformationService;
            _appServiceProvider = appServiceProvider;
            ConnectionData = _userInformationService.ConnectionData;


        }

        public void InitializeCommands()
        {
            _initializeCommand = ReactiveCommand.CreateFromTask(_ => GetProjects());
            _updateTicketCommand = ReactiveCommand.CreateFromTask(_ => UpdateTicket(), CanUpdateTicket());
            _updateCommitCommand = ReactiveCommand.CreateFromTask(_ => UpdateCommit(), CanUpdateCommit());
        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            this.WhenAnyValue(x => x.SelectedProject).Subscribe(async _ => await GetTickets());

            //_appServiceProvider.GetService<ITeamExplorerPage>();
            yield break;
        }



        private IObservable<bool> CanUpdateCommit()
        {
            return Observable.Return(true);
        }

        private Task UpdateCommit()
        {
            throw new NotImplementedException();
        }

        private IObservable<bool> CanUpdateTicket()
        {
            return Observable.Return(true);

        }

        private Task UpdateTicket()
        {
            throw new NotImplementedException();
        }


        private async Task GetTickets()
        {
            if (SelectedProject == null)
                return;

            var issues = await _youTrackClientService.GetIssuesByProject(SelectedProject.ShortName);
            LocalIssues = issues
                .OrderBy(x => x.Id)
                .ToList();
            SelectedIssue = LocalIssues.FirstOrDefault();
        }

        private async Task GetProjects()
        {
            if (!ConnectionData.IsLoggedIn)
                return;

            var project = await _youTrackClientService.GetAllProjects();
            LocalProjects = project
                .OrderBy(x => x.Name)
                .ToList();
            SelectedProject = LocalProjects.FirstOrDefault();
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public IEnumerable<ReactiveCommand> ThrowableCommands =>
            new[] { _initializeCommand, _updateCommitCommand, _updateTicketCommand };

        public IEnumerable<ReactiveCommand> LoadingCommands =>
            new[] { _initializeCommand, _updateCommitCommand, _updateTicketCommand };
    }
}