using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IConnectSectionViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ConnectSectionViewModel : ViewModelBase, IConnectSectionViewModel
    {
        private readonly ILog _logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExportFactory<ILoginDialogView> _loginViewFactory;
        private readonly IEventAggregatorService _eventAggregator;
        private readonly IUserInformationService _userInformationService;
        private readonly IYouTrackClientService _youTrackClientService;
        private readonly ICommandsService _commandsService;
        private ReactiveCommand _openLoginCommand;
        private ReactiveCommand _logoutCommand;
        private ReactiveCommand _openCloneCommand;
        private ReactiveCommand _openCreateCommand;
        private ReactiveCommand _initializeCommand;

        private ConnectionData _connectionData;
        private List<YouTrackProject> _localProjects;
        private YouTrackProject _selectProject;
        private readonly IVsTools _vsTools;
        private readonly ITeamExplorerCommandsService _teamExplorerCommandsService;

        public ICommand OpenLoginCommand => _openLoginCommand;
        public ICommand OpenCreateCommand => _openCreateCommand;
        public ICommand LogoutCommand => _logoutCommand;
        public ICommand InitializeCommand => _initializeCommand;

        public YouTrackProject SelectedProject
        {
            get => _selectProject;
            set
            {
                var previousProject = _selectProject;
                this.RaiseAndSetIfChanged(ref _selectProject, value);
                _eventAggregator.Publish(new ActiveProjectChangedEvent(value, previousProject));
            }
        }


        public ConnectionData ConnectionData
        {
            get => _connectionData;
            set => this.RaiseAndSetIfChanged(ref _connectionData, value);
        }

        public List<YouTrackProject> LocalProjects
        {
            get => _localProjects;
            set => this.RaiseAndSetIfChanged(ref _localProjects, value);
        }

        public string ErrorMessage { get; set; }

        public IEnumerable<ReactiveCommand> ThrowableCommands => new[] { _initializeCommand };


        [ImportingConstructor]
        public ConnectSectionViewModel(
            ExportFactory<ILoginDialogView> loginViewFactory,
            //ExportFactory<ICloneRepositoriesDialogView> cloneRepoViewFactory,
            //ExportFactory<ICreateRepositoriesDialogView> createRepoViewFactory,
            IEventAggregatorService eventAggregator,
            IUserInformationService userInformationService,
            IYouTrackClientService youTrackClientService,
            IVsTools vsTools,
            ITeamExplorerCommandsService teamExplorerCommandsService,
                ICommandsService commandsService)
        {
            _loginViewFactory = loginViewFactory;
            // _cloneRepoViewFactory = cloneRepoViewFactory;
            // _createRepoViewFactory = createRepoViewFactory;
            _eventAggregator = eventAggregator;
            _userInformationService = userInformationService;
            _youTrackClientService = youTrackClientService;
            _vsTools = vsTools;
            _teamExplorerCommandsService = teamExplorerCommandsService;
            _commandsService = commandsService;
            ConnectionData = _userInformationService.ConnectionData;

        }

        public void InitializeCommands()
        {
            _openLoginCommand = ReactiveCommand.Create(() => _loginViewFactory.CreateExport().Value.ShowDialog());

            _logoutCommand = ReactiveCommand.Create(() => { _youTrackClientService.Logout(); });
            _initializeCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!ConnectionData.IsLoggedIn)
                    return;


                var localRepositories = await _youTrackClientService.GetAllProjects(true);

                LocalProjects = localRepositories
                    .OrderBy(x => x.Name)
                    .ToList();

                if (_userInformationService.ClientHistory.ActiveProject != null)
                {
                    SelectedProject = LocalProjects.FirstOrDefault(l =>
                        l.Name == _userInformationService.ClientHistory.ActiveProject.Name);
                }

            });
        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            yield return _eventAggregator.GetEvent<ConnectionChangedEvent>().Subscribe(ConnectionChanged);

            yield return _eventAggregator.GetEvent<ActiveRepositoryChangedEvent>()
                .Select(x => Unit.Default)
                .InvokeCommand(_initializeCommand);

            yield return _eventAggregator.GetEvent<ConnectionChangedEvent>()
                .Select(x => Unit.Default)
                .InvokeCommand(_initializeCommand);

            yield return _eventAggregator.GetEvent<ClonedRepositoryEvent>()
                .Select(x => Unit.Default)
                .InvokeCommand(_initializeCommand);
        }

        private void ConnectionChanged(ConnectionChangedEvent connectionChangedEvent)
        {
            ConnectionData = connectionChangedEvent.Data;
        }


        public void ChangeActiveProject()
        {
            if (SelectedProject != null)
            {
                _commandsService.ShowYouTrackIssuesWindow();
                // _eventAggregator.Publish(new ActiveProjectChangedEvent());
            }
        }
    }
}