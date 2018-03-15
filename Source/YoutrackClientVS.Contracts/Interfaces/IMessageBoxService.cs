using System.Windows.Input;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface IMessageBoxService
    {
        void ExecuteCommandWithConfirmation(string title, string message, ICommand command);
    }
}
