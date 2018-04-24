using log4net;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfControls;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(ICreateIssueViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreateIssueViewModel : ViewModelBase, ICreateIssueViewModel
    {
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEventAggregatorService _eventAggregator;

        //private readonly IGitService _gitService;
        private readonly IPageNavigationService<IYouTrackIssuesWindow> _pageNavigationService;

        private readonly IUserInformationService _userInfoService;

        // private readonly ITreeStructureGenerator _treeStructureGenerator;
        private readonly IVsTools _vsTools;

        private readonly IYouTrackClientService _youTrackClientService;

        //private IEnumerable<GitBranch> _branches;
        //private bool _closeSourceBranch;
        private ReactiveCommand _createNewUssueCommand;

        private IDataNotifier _dataNotifier;

        private string _description;

        //private GitBranch _destinationBranch;
        private string _errorMessage;
        private ReactiveCommand _goToDetailsCommand;
        private ReactiveCommand _initializeCommand;
        private bool _isLoading;
        private List<YouTrackProject> _localProjects;
        private string _message;
        private GitPullRequest _remotePullRequest;
        private ReactiveCommand _removeReviewerCommand;
        private ReactiveList<GitUser> _selectedReviewers;
        private YouTrackProject _selectProject;
        private ReactiveCommand<Unit, Unit> _setPullRequestDataCommand;
        private GitBranch _sourceBranch;
        private string _title;

        [ImportingConstructor]
        public CreateIssueViewModel(
            IYouTrackClientService youTrackClientService,
            IPageNavigationService<IYouTrackIssuesWindow> pageNavigationService,
            IEventAggregatorService eventAggregator,
            IUserInformationService userInfoService,
            ICommandsService commandsService,
            IDataNotifier dataNotifier,
            IPullRequestDiffViewModel pullRequestDiffViewModel
        )
        {
            _youTrackClientService = youTrackClientService;

            _pageNavigationService = pageNavigationService;
            _eventAggregator = eventAggregator;

            _dataNotifier = dataNotifier;

            PullRequestDiffViewModel = pullRequestDiffViewModel;

            _userInfoService = userInfoService;
            SelectedReviewers = new ReactiveList<GitUser>();
        }

        public YouTrackProject SelectedProject
        {
            get => _selectProject;
            set => this.RaiseAndSetIfChanged(ref _selectProject, value);
        }

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        [Required]
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public List<YouTrackProject> LocalProjects
        {
            get => _localProjects;
            set => this.RaiseAndSetIfChanged(ref _localProjects, value);
        }

        public ReactiveList<GitUser> SelectedReviewers
        {
            get => _selectedReviewers;
            set => this.RaiseAndSetIfChanged(ref _selectedReviewers, value);
        }

        public GitPullRequest RemotePullRequest
        {
            get => _remotePullRequest;
            set => this.RaiseAndSetIfChanged(ref _remotePullRequest, value);
        }

        public ISuggestionProvider ReviewersProvider => new SuggestionProvider(Filter);


        public IPullRequestDiffViewModel PullRequestDiffViewModel { get; set; }

        public string ExistingBranchText => RemotePullRequest == null
            ? null
            : $"#{RemotePullRequest.Id} {RemotePullRequest.Title} (created {RemotePullRequest.Created})";

        public ICommand CreateNewUssueCommand => _createNewUssueCommand;
        public ICommand RemoveReviewerCommand => _removeReviewerCommand;
        public ICommand GoToDetailsCommand => _goToDetailsCommand;

        public string PageTitle { get; } = "Create New Issue";


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
            new[] { _initializeCommand, _createNewUssueCommand, _setPullRequestDataCommand }.Concat(
                PullRequestDiffViewModel.ThrowableCommands);

        public IEnumerable<ReactiveCommand> LoadingCommands => new[]
            {_initializeCommand, _createNewUssueCommand, _setPullRequestDataCommand};

        public ICommand InitializeCommand => _initializeCommand;

        public void InitializeCommands()
        {
            _initializeCommand = ReactiveCommand.CreateFromTask(_ => LoadBranchesAsync(), CanLoadPullRequests());
            _removeReviewerCommand = ReactiveCommand.Create<GitUser>(x => SelectedReviewers.Remove(x));
            _createNewUssueCommand =
                ReactiveCommand.CreateFromTask(_ => CreateNewIssue(), CanCreateIssue());
            _setPullRequestDataCommand =
                ReactiveCommand.CreateFromTask(_ => SetPullRequestDataAsync(), Observable.Return(true));
            _goToDetailsCommand =
                ReactiveCommand.Create<long>(id => _pageNavigationService.Navigate<IYouTrackIssueDetailView>(id));
        }


        protected override IEnumerable<IDisposable> SetupObservables()
        {
            yield return _eventAggregator.GetEvent<ActiveRepositoryChangedEvent>()
                .Select(x => Unit.Default)
                .InvokeCommand(_initializeCommand);
        }


        private async Task CreateNewIssue()
        {
            //var gitPullRequest = new GitPullRequest(Title, Description, SourceBranch, DestinationBranch)
            //{
            //    Id = RemotePullRequest?.Id ?? 0,
            //    CloseSourceBranch = CloseSourceBranch,
            //    Reviewers = SelectedReviewers.ToDictionary(x => x, x => true),
            //    Version = RemotePullRequest?.Version,
            //};

            //if (RemotePullRequest == null)
            //    await _youTrackClientService.CreateIssue(gitPullRequest);
            //else
            //    await _youTrackClientService.UpdatePullRequest(gitPullRequest);
            var youtrackIssue = new YouTrackIssue
            {
                Summary = Title,
                Description = Description,
                ProjectShortName = SelectedProject.ShortName
            };

            await _youTrackClientService.CreateIssue(SelectedProject.ShortName, youtrackIssue);
            _dataNotifier.ShouldUpdate = true;
            _pageNavigationService.NavigateBack(true);
        }

        private async Task LoadBranchesAsync()
        {
            var localRepositories = await _youTrackClientService.GetAllProjects(true);

            LocalProjects = localRepositories
                .OrderBy(x => x.Name)
                .ToList();
            SelectedProject =
                LocalProjects.FirstOrDefault(l => l.Name == _userInfoService.ClientHistory.ActiveProject.Name);
            Message = _selectProject != null && string.IsNullOrEmpty(_selectProject.Name)
                ? $"Warning! Your active local branch {_selectProject.Name} is not tracking any remote branches."
                : string.Empty;
        }

        private async Task SetPullRequestDataAsync()
        {
            //int? pullRequest = null;

            //PullRequestDiffViewModel.AddCommits(commits);

            //if (pullRequest != null)
            //{
            //    Title = pullRequest.Title;
            //    Description = pullRequest.Description;
            //    SelectedReviewers.Clear();
            //    foreach (var reviewer in pullRequest.Reviewers.Where(x => x.Key.Username != pullRequest.Author.Username)
            //        .Select(x => x.Key))
            //        SelectedReviewers.Add(reviewer);
            //}
            //else
            //{
            //    await SetPullRequestDataFromCommits(PullRequestDiffViewModel.Commits);
            //}

            //RemotePullRequest = pullRequest;
            //this.RaisePropertyChanged(nameof(ExistingBranchText));
        }


        private IObservable<bool> CanLoadPullRequests()
        {
            return Observable.Return(true);
        }

        private IObservable<bool> CanCreateIssue()
        {
            return ValidationObservable.Select(x => Unit.Default)
                .Merge(Changed.Select(x => Unit.Default))
                .Select(x => CanExecute()).StartWith(CanExecute());
        }

        private bool CanExecute()
        {
            return IsObjectValid() &&
                   !string.IsNullOrEmpty(_selectProject?.Name);
        }


        private IEnumerable Filter(string arg)
        {
            if (arg.Length < 3)
                return Enumerable.Empty<GitUser>();

            try
            {
                var suggestions = _youTrackClientService.GetRepositoryUsers(arg).Result;
                return suggestions.Except(SelectedReviewers, x => x.Username).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Enumerable.Empty<GitUser>();
            }
        }

        private async Task SetPullRequestDataFromCommits(List<GitCommit> commits)
        {
            Title = _selectProject?.Name;
            Description = string.Join(Environment.NewLine, commits.Select((x) => $"* " + x.Message?.Trim()).Reverse());
            SelectedReviewers.Clear();
            foreach (var defReviewer in await _youTrackClientService.GetDefaultReviewers())
                SelectedReviewers.Add(defReviewer);
        }
    }
}