using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using YouTrackClientVS.Contracts;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.AutoCompleteTextBox;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IYouTrackIssuesMainViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class YouTrackIssuesMainViewModel : ViewModelBase, IYouTrackIssuesMainViewModel
    {
        private readonly IUserInformationService _userInfoService;
        private readonly IYouTrackClientService _youTrackClientService;
        private readonly IAutoCompleteIntellisenseQuerySource _autoCompleteIntellisenseQuerySource;
        private readonly IEventAggregatorService _eventAggregator;
        private readonly IPageNavigationService<IYouTrackIssuesWindow> _pageNavigationService;
        private readonly IUserInformationService _userInformationService;
        private readonly IEventAggregatorService _eventAggregatorService;
        private Theme _currentTheme;

        private ReactiveCommand _initializeCommand;
        private ReactiveCommand _goToDetailsCommand;
        private ReactiveCommand _loadNextPageCommand;
        private ReactiveCommand _goToCreateNewIssueCommand;
        private ReactiveCommand _refreshIssuesCommand;

        private ReactiveCommand _autoCompleteTextBoxCommand;
        //private YouTrackIntellisense _intellisense;
        //private YouTrackSuggestItem _selectedSuggest;
        //private string _intellisensSearch;
        private bool _isLoading;
        private string _errorMessage;
        private string _intellisenseSearchQuery;

        private YouTrackStatusSearch? _selectedStatus;
        private YouTrackIssue _selectedIssue;
        private PagedCollection<YouTrackIssue> _youTrackIssues;
        private readonly IDataNotifier _dataNotifier;


        public YouTrackStatusSearch? SelectedStatus
        {
            get => _selectedStatus;
            set => this.RaiseAndSetIfChanged(ref _selectedStatus, value);
        }


        public string IntellisenseSearchQuery
        {
            get => _intellisenseSearchQuery;
            set => this.RaiseAndSetIfChanged(ref _intellisenseSearchQuery, value);
        }

        //public string IntellisensSearch
        //{
        //    get => _intellisensSearch;
        //    set => this.RaiseAndSetIfChanged(ref _intellisensSearch, value);
        //}

        //public YouTrackIntellisense Intellisense
        //{
        //    get => _intellisense;
        //    set => this.RaiseAndSetIfChanged(ref _intellisense, value);
        //}

        //public YouTrackSuggestItem SelectedSuggest
        //{
        //    get => _selectedSuggest;
        //    set
        //    {
        //        this.RaiseAndSetIfChanged(ref _selectedSuggest, value);
        //        IntellisensSearch += $" {value.Option}";
        //    }
        //}

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

        public IEnumerable<ReactiveCommand> ThrowableCommands =>
            new[] { _initializeCommand, _loadNextPageCommand, _refreshIssuesCommand, _autoCompleteTextBoxCommand };

        public IEnumerable<ReactiveCommand> LoadingCommands =>
            new[] { _initializeCommand, _loadNextPageCommand, _refreshIssuesCommand };

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
        public ICommand AutoCompleteTextBoxCommand => _autoCompleteTextBoxCommand;


        public Theme CurrentTheme
        {
            get => _currentTheme;
            set => this.RaiseAndSetIfChanged(ref _currentTheme, value);
        }

        //public ISuggestionProvider SuggestProvider
        //{
        //    get { return new SuggestionProvider(x => Intellisense.Suggest); }
        //}

        private const int PageSize = 50;
        private bool _isInitialized = false;


        public AutoCompleteQueryResultProvider AutoCompleteQueryResultProvider => new AutoCompleteQueryResultProvider(_autoCompleteIntellisenseQuerySource
            .QueryResultFunction);

        [ImportingConstructor]
        public YouTrackIssuesMainViewModel(
            IYouTrackClientService youTrackClientService,
            IPageNavigationService<IYouTrackIssuesWindow> pageNavigationService,
            IUserInformationService userInfoService,
            IDataNotifier dataNotifier,
            IEventAggregatorService eventAggregator,
            IAutoCompleteIntellisenseQuerySource autoCompleteIntellisenseQuerySource,
            IUserInformationService userInformationService,
            IEventAggregatorService eventAggregatorService
        )
        {
            _autoCompleteIntellisenseQuerySource = autoCompleteIntellisenseQuerySource;
            _youTrackClientService = youTrackClientService;
            _pageNavigationService = pageNavigationService;
            _dataNotifier = dataNotifier;
            _userInfoService = userInfoService;
            _eventAggregator = eventAggregator;
            SelectedStatus = YouTrackStatusSearch.Open;
            _userInformationService = userInformationService;
            CurrentTheme = userInformationService.CurrentTheme;
            _eventAggregatorService = eventAggregatorService;

        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            yield return _eventAggregatorService.GetEvent<ThemeChangedEvent>()
                .Subscribe(ev => { CurrentTheme = ev.Theme; });
            this.WhenAnyValue(x => x.SelectedStatus)
                .Select(x => new { SelectedStatus })
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

            _goToCreateNewIssueCommand =
                ReactiveCommand.Create(() => { _pageNavigationService.Navigate<ICreateIssueView>(); },
                    Observable.Return(true));
            _goToDetailsCommand =
                ReactiveCommand.Create<YouTrackIssue>(x =>
                    _pageNavigationService.Navigate<IYouTrackIssueDetailView>(x.Id));
            _loadNextPageCommand = ReactiveCommand.CreateFromTask(_ => YouTrackIssues.LoadNextPageAsync());
            _refreshIssuesCommand = ReactiveCommand.CreateFromTask(_ => RefreshIssuesAsync());
            _autoCompleteTextBoxCommand = ReactiveCommand.CreateFromTask<string>(async param => await AutoCompleteTextBoxActionAsync(param));
        }



        private async Task AutoCompleteTextBoxActionAsync(string param)
        {
            await InitPagedCollectionAsync();
        }

        private async Task RefreshIssuesAsync()
        {
            IntellisenseSearchQuery = string.Empty;
            await InitPagedCollectionAsync();
        }

        private async Task InitPagedCollectionAsync()
        {
            _dataNotifier.ShouldUpdate = false;
            //Authors = (await _youTrackClientService.GetUsers()).ToList();
            YouTrackIssues = new PagedCollection<YouTrackIssue>(GetPullRequestsPageAsync, PageSize);
            //Intellisense = await _youTrackClientService.GetIntellisense(null, null);
            await YouTrackIssues.LoadNextPageAsync();
        }

        private async Task<IEnumerable<YouTrackIssue>> GetPullRequestsPageAsync(int pageSize, int page)
        {
            var lst = await _youTrackClientService.GetIssuesPage(
                state: SelectedStatus,
                project: _userInfoService.ClientHistory.ActiveProject?.ShortName,
                limit: pageSize,
                page: page,
                filter: IntellisenseSearchQuery
            );

            return lst;
        }

        private IObservable<bool> CanLoadPullRequests()
        {
            return this.WhenAnyValue(x => x.IsLoading).Select(x => !IsLoading);
        }
    }
}