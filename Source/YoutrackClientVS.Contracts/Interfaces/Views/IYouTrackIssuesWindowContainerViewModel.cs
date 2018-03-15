using System;

namespace YouTrackClientVS.Contracts.Interfaces.Views
{
    public interface IYouTrackIssuesWindowContainerViewModel : IViewModel
    {
        event EventHandler Closed;
    }
}