using System.Windows.Input;

namespace YouTrackClientVS.Contracts.Interfaces.ViewModels
{
    public interface ILoginDialogViewModel : ICloseable, IViewModelWithErrorMessage, ILoadableViewModel, IViewModel
    {
        string Login { get; set; }
        string Password { get; set; }
        ICommand ConnectCommand { get; }
    }
}