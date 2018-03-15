using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(ILoginDialogViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LoginDialogViewModel : ViewModelBase, ILoginDialogViewModel
    {
        private readonly IYouTrackClientService _youTrackClientService;
        private string _login;
        private string _password;
        private ReactiveCommand _connectCommand;
        private string _errorMessage;
        private bool _isLoading;
        private string _host;


        public ICommand ConnectCommand => _connectCommand;
        public IEnumerable<ReactiveCommand> ThrowableCommands => new List<ReactiveCommand> { _connectCommand };
        public IEnumerable<ReactiveCommand> LoadingCommands => new[] { _connectCommand };

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        [Required]
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        [Required]
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        [ValidatesViaMethod(AllowBlanks = true, AllowNull = true, Name = nameof(ValidateHost),
            ErrorMessage = "Url is not valid. It must include schema.")]
        public string Host
        {
            get => _host;
            set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }


        [ImportingConstructor]
        public LoginDialogViewModel(IYouTrackClientService youTrackClientService)
        {
            _youTrackClientService = youTrackClientService;
        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            this.WhenAnyValue(x => x.Host).Subscribe(_ => ForcePropertyValidation(nameof(Host)));
            yield break;
        }


        public void InitializeCommands()
        {
            _connectCommand = ReactiveCommand.CreateFromTask(_ => Connect(), CanExecuteObservable());
        }

        private async Task Connect()
        {
            var cred = new YouTrackCredentials()
            {
                Login = Login,
                Password = Password,
                Host = new Uri(Host)

            };

            await _youTrackClientService.LoginAsync(cred);
            OnClose();
        }


        private IObservable<bool> CanExecuteObservable()
        {
            return ValidationObservable.Select(x => Unit.Default)
                .Merge(Changed.Select(x => Unit.Default))
                .Select(x => CanExecute()).StartWith(CanExecute());
        }

        private bool CanExecute()
        {
            return IsObjectValid();
        }

        public bool ValidateHost(string host)
        {
            return Uri.TryCreate(host, UriKind.Absolute, out var outUri) &&
                   (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }

        protected void OnClose()
        {
            Closed?.Invoke(this, new EventArgs());
        }

        public event EventHandler Closed;
    }
}