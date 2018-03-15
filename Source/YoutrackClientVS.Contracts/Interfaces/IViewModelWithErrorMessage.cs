using System.Collections.Generic;
using ReactiveUI;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface IViewModelWithErrorMessage : IViewModelWithCommands
    {
        string ErrorMessage { get; set; }
        IEnumerable<ReactiveCommand> ThrowableCommands { get; }
    }
}