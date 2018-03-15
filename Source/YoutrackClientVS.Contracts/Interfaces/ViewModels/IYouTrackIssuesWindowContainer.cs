using YouTrackClientVS.Contracts.Interfaces.Views;

namespace YouTrackClientVS.Contracts.Interfaces.ViewModels
{
    public interface IYouTrackIssuesWindowContainer : IView
    {
        IYouTrackIssuesWindow Window { get; set; }
    }
}