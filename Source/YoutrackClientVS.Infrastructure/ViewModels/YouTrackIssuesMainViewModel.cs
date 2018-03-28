using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfControls;
using YouTrackClientVS.Contracts;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.Extensions;
using SuggestionProvider = WpfControls.SuggestionProvider;
namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IYouTrackIssuesMainViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class YouTrackIssuesMainViewModel : ViewModelBase, IYouTrackIssuesMainViewModel
    {
        private readonly IUserInformationService _userInfoService;
        private readonly IYouTrackClientService _youTrackClientService;
        private readonly IEventAggregatorService _eventAggregator;
        private readonly IPageNavigationService<IYouTrackIssuesWindow> _pageNavigationService;
        private ReactiveCommand _initializeCommand;
        private ReactiveCommand _goToDetailsCommand;
        private ReactiveCommand _loadNextPageCommand;
        private ReactiveCommand _goToCreateNewIssueCommand;
        private ReactiveCommand _refreshIssuesCommand;
        private bool _isLoading;
        private string _errorMessage;

        private List<YouTrackUser> _authors;
        private YouTrackUser _selectedAuthor;
        private YouTrackStatusSearch? _selectedStatus;
        private YouTrackIssue _selectedIssue;
        private PagedCollection<YouTrackIssue> _youTrackIssues;
        private readonly IDataNotifier _dataNotifier;

        public YouTrackUser SelectedAuthor
        {
            get => _selectedAuthor;
            set => this.RaiseAndSetIfChanged(ref _selectedAuthor, value);
        }

        public YouTrackStatusSearch? SelectedStatus
        {
            get => _selectedStatus;
            set => this.RaiseAndSetIfChanged(ref _selectedStatus, value);
        }

        public List<YouTrackUser> Authors
        {
            get => _authors;
            set => this.RaiseAndSetIfChanged(ref _authors, value);
        }

        public YouTrackIssue SelectedIssue
        {
            get => _selectedIssue;
            set => this.RaiseAndSetIfChanged(ref _selectedIssue, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public IEnumerable<ReactiveCommand> ThrowableCommands => new[] { _initializeCommand, _loadNextPageCommand, _refreshIssuesCommand };
        public IEnumerable<ReactiveCommand> LoadingCommands => new[] { _initializeCommand, _loadNextPageCommand, _refreshIssuesCommand };

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public PagedCollection<YouTrackIssue> YouTrackIssues
        {
            get => _youTrackIssues;
            set => this.RaiseAndSetIfChanged(ref _youTrackIssues, value);
        }

        public string PageTitle { get; } = "YouTrack Issues";

        public ICommand InitializeCommand => _initializeCommand;
        public ICommand GoToDetailsCommand => _goToDetailsCommand;
        public ICommand GotoCreateNewIssueCommand => _goToCreateNewIssueCommand;
        public ICommand LoadNextPageCommand => _loadNextPageCommand;
        public ICommand RefreshIssuesCommand => _refreshIssuesCommand;

        public ISuggestionProvider AuthorProvider
        {
            get
            {
                return new SuggestionProvider(x => Authors.Where(y =>
                (y.FullName != null && y.FullName.Contains(x, StringComparison.InvariantCultureIgnoreCase)) ||
                (y.Login != null && y.Login.Contains(x, StringComparison.InvariantCultureIgnoreCase))));
            }
        }

        private const int PageSize = 50;
        private bool _isInitialized = false;

        [ImportingConstructor]
        public YouTrackIssuesMainViewModel(
            IYouTrackClientService youTrackClientService,
            IPageNavigationService<IYouTrackIssuesWindow> pageNavigationService,
            IUserInformationService userInfoService,
            IDataNotifier dataNotifier,
            IEventAggregatorService eventAggregator
            )
        {
            _youTrackClientService = youTrackClientService;
            _pageNavigationService = pageNavigationService;
            _dataNotifier = dataNotifier;
            _userInfoService = userInfoService;
            _eventAggregator = eventAggregator;
            SelectedStatus = YouTrackStatusSearch.Open;
            Authors = new List<YouTrackUser>();
        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            this.WhenAnyValue(x => x.SelectedStatus, x => x.SelectedAuthor)
                .Select(x => new { SelectedStatus, SelectedAuthor })
                .DistinctUntilChanged()
                .Where(x => _isInitialized)
                .Where(x => !IsLoading)
                .Select(x => Unit.Default)
                .InvokeCommand(_refreshIssuesCommand);

            this.WhenAnyValue(x => x.SelectedIssue)
                .Where(x => x != null)
                .InvokeCommand(_goToDetailsCommand);
            yield return _eventAggregator
                .GetEvent<ActiveProjectChangedEvent>()
                .Where(x => x.IsProjectDifferent)
                .Select(x => Unit.Default)
                .Merge(_eventAggregator.GetEvent<ConnectionChangedEvent>().Select(x => Unit.Default))
                .Subscribe(async _ => await RefreshIssuesAsync());
            yield break;
        }

        public void InitializeCommands()
        {
            _initializeCommand = ReactiveCommand.CreateFromTask(async _ =>
            {
                if (_isInitialized && !_dataNotifier.ShouldUpdate)
                    return;


                await RefreshIssuesAsync();
                _isInitialized = true;
            }, CanLoadPullRequests());

            _goToCreateNewIssueCommand = ReactiveCommand.Create(() => { _pageNavigationService.Navigate<ICreateIssueView>(); }, Observable.Return(false));
            _goToDetailsCommand = ReactiveCommand.Create<YouTrackIssue>(x => _pageNavigationService.Navigate<IYouTrackIssueDetailView>(x.Id));
            _loadNextPageCommand = ReactiveCommand.CreateFromTask(_ => YouTrackIssues.LoadNextPageAsync());
            _refreshIssuesCommand = ReactiveCommand.CreateFromTask(_ => RefreshIssuesAsync());
        }

        private async Task RefreshIssuesAsync()
        {
            _dataNotifier.ShouldUpdate = false;
            Authors = (await _youTrackClientService.GetUsers()).ToList();
            YouTrackIssues = new PagedCollection<YouTrackIssue>(GetPullRequestsPageAsync, PageSize);
            var res = (await _youTrackClientService.GetIntellisense(null, null));
            await YouTrackIssues.LoadNextPageAsync();
        }

        private async Task<IEnumerable<YouTrackIssue>> GetPullRequestsPageAsync(int pageSize, int page)
        {
            var lst = await _youTrackClientService.GetIssuesPage(
                state: SelectedStatus,
                project: _userInfoService.ClientHistory.ActiveProject?.ShortName,
                author: SelectedAuthor?.Login,
                limit: pageSize,
                page: page
                );

            return lst;
        }

        private IObservable<bool> CanLoadPullRequests()
        {
            return this.WhenAnyValue(x => x.IsLoading).Select(x => !IsLoading);
        }
    }
}
