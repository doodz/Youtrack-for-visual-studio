using System.Collections.Generic;
using ReactiveUI;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface ILoadableViewModel: IViewModelWithCommands
    {
        bool IsLoading { get; set; }
        IEnumerable<ReactiveCommand> LoadingCommands { get; }
    }
}
