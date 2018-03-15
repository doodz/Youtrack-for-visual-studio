using System;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Contracts.Interfaces.ViewModels
{
    public interface IConnectSectionViewModel : IViewModel, IViewModelWithErrorMessage, IInitializable, IDisposable
    {
        void ChangeActiveProject();
        YouTrackProject SelectedProject { get; set; }
    }
}
