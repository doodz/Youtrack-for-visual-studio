using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IYouTrackIssuesWindowContainerViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class YouTrackIssuesWindowContainerViewModel : ViewModelBase, IYouTrackIssuesWindowContainerViewModel
    {
        private readonly IPageNavigationService<IYouTrackIssuesWindow> _pageNavigationService;
        private readonly IEventAggregatorService _eventAggregator;

        private readonly ICacheService _cacheService;
        private readonly IUserInformationService _userInfoService;
        private IView _currentView;
        private readonly ReactiveCommand<Unit, Unit> _prevCommand;
        private readonly ReactiveCommand<Unit, Unit> _nextCommand;
        private IWithPageTitle _currentViewModel;
        private Theme _currentTheme;

        public ShowConfirmationEventViewModel ConfirmationViewModel { get; }
        public string ActiveProject { get; set; }

        public IView CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public IWithPageTitle CurrentViewModel
        {
            get => _currentViewModel;
            set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        public Theme CurrentTheme
        {
            get => _currentTheme;
            set => this.RaiseAndSetIfChanged(ref _currentTheme, value);
        }

        public ICommand PrevCommand => _prevCommand;
        public ICommand NextCommand => _nextCommand;

        [ImportingConstructor]
        public YouTrackIssuesWindowContainerViewModel(
        IPageNavigationService<IYouTrackIssuesWindow> pageNavigationService,
        IEventAggregatorService eventAggregator,
        ICacheService cacheService,
        IUserInformationService userInfoService
        )
        {
            _pageNavigationService = pageNavigationService;
            _eventAggregator = eventAggregator;

            _cacheService = cacheService;
            _userInfoService = userInfoService;

            _prevCommand = ReactiveCommand.Create(() => _pageNavigationService.NavigateBack(),
                _pageNavigationService.CanNavigateBackObservable);
            _nextCommand = ReactiveCommand.Create(() => _pageNavigationService.NavigateForward(),
                _pageNavigationService.CanNavigateForwardObservable);

            //TODO THE change that.
            ActiveProject = _userInfoService.ClientHistory.ActiveProject?.Name ?? "All projects";

            CurrentTheme = _userInfoService.CurrentTheme;
            ConfirmationViewModel = new ShowConfirmationEventViewModel();
        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            yield return _pageNavigationService.Where(x => x.Window == typeof(IYouTrackIssuesWindow))
                .Subscribe(ChangeView);
            yield return _eventAggregator.GetEvent<ThemeChangedEvent>().Subscribe(ev => CurrentTheme = ev.Theme);
            yield return _eventAggregator.GetEvent<ShowConfirmationEvent>().Subscribe(ShowConfirmation);
            yield return _eventAggregator
                .GetEvent<ActiveProjectChangedEvent>()
                .Where(x => x.IsProjectDifferent)
                .Select(x => Unit.Default)
                .Merge(_eventAggregator.GetEvent<ConnectionChangedEvent>().Select(x => Unit.Default))
                .Subscribe(_ => OnClosed());

            this.WhenAnyValue(x => x.CurrentView).Where(x => x != null)
                .Subscribe(_ => CurrentViewModel = CurrentView.DataContext as IWithPageTitle);

            this.WhenAnyObservable(x => x._nextCommand, x => x._prevCommand)
                .Subscribe(_ => ConfirmationViewModel.Event = null);
        }

        private void ChangeView(NavigationEvent navEvent)
        {
            CurrentView = navEvent.View;
        }

        public event EventHandler Closed;

        protected virtual void OnClosed()
        {
            _cacheService.Delete(CacheKeys.PullRequestCacheKey);
            _pageNavigationService.ClearNavigationHistory();
            Closed?.Invoke(this, EventArgs.Empty);
            Dispose();
        }

        private void ShowConfirmation(ShowConfirmationEvent ev)
        {
            ConfirmationViewModel.Event = ev;
        }
    }
}