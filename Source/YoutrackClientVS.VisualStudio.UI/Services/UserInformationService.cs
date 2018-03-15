using System;
using System.ComponentModel.Composition;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.PlatformUI;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.UI.Helpers;

namespace YouTrackClientVS.VisualStudio.UI.Services
{
    [Export(typeof(IUserInformationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class UserInformationService : IUserInformationService
    {
        private readonly IEventAggregatorService _eventAggregator;
        private readonly IStorageService _storageService;
        private IDisposable _observable;
        private IDisposable _observableHistory;
        private IDisposable _themeObs;
        private IDisposable _observableProject;

        public ConnectionData ConnectionData { get; private set; } = ConnectionData.NotLogged;
        public YouTrackClientHistory ClientHistory { get; private set; } = YouTrackClientHistory.Default;
        public Theme CurrentTheme { get; private set; } = ConvertToTheme(VSHelpers.DetectTheme());

        [ImportingConstructor]
        public UserInformationService(
            IEventAggregatorService eventAggregator,
            IStorageService storageService)
        {
            _eventAggregator = eventAggregator;
            _storageService = storageService;
        }

        public void StartListening()
        {
            _observable = _eventAggregator.GetEvent<ConnectionChangedEvent>().Subscribe(ConnectionChanged);
            _observableHistory = _eventAggregator.GetEvent<ClientHistoryChangedEvent>().Subscribe(ClientHistoryChanged);
            _observableProject = _eventAggregator.GetEvent<ActiveProjectChangedEvent>().Subscribe(ActiveProjectChanged);

            _themeObs = Observable
                .FromEvent<ThemeChangedEventHandler, ThemeChangedEventArgs>(handler => VSColorTheme.ThemeChanged += handler, handler => VSColorTheme.ThemeChanged -= handler)
                .Subscribe(args =>
                {
                    CurrentTheme = ConvertToTheme(VSHelpers.DetectTheme());
                    _eventAggregator.Publish(new ThemeChangedEvent(CurrentTheme));
                });
        }


        public async Task Initialize()
        {
            var resultData = _storageService.LoadYouTrackClientHistory();
            if(resultData.IsSuccess)
                ClientHistory = resultData.Data;
        }

        private static Theme ConvertToTheme(string theme)
        {
            return theme == "Dark" ? Theme.Dark : Theme.Light;
        }

        public void Dispose()
        {
            _observable?.Dispose();
            _themeObs?.Dispose();
            _observableHistory?.Dispose();
            _observableProject?.Dispose();
        }

        private void ConnectionChanged(ConnectionChangedEvent connectionChangedEvent)
        {
            ConnectionData = connectionChangedEvent.Data;
            _storageService.SaveUserData(ConnectionData);
        }


        private void ActiveProjectChanged(ActiveProjectChangedEvent activeProjectChangedEvent)
        {
            ClientHistory.ActiveProject = activeProjectChangedEvent.ActiveProject;
            ClientHistory.PreviousProject = activeProjectChangedEvent.PreviousProject;
            var result = _storageService.SaveYouTrackClientHistory(ClientHistory);
        }

        private void ClientHistoryChanged(ClientHistoryChangedEvent connectionChangedEvent)
        {
            ClientHistory = connectionChangedEvent.Data;
            var result = _storageService.SaveYouTrackClientHistory(ClientHistory);
        }
    }
}
