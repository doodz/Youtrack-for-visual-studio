using System.Windows.Input;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface IInitializable
    {
        ICommand InitializeCommand { get; }
    }
}
