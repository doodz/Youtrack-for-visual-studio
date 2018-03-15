using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Contracts.Interfaces.ViewModels
{
    public interface IYouTrackIssueDetailViewModel : IInitializable, IViewModelWithErrorMessage, ILoadableViewModel, IWithPageTitle
    {
        YouTrackIssue Issue { get; set; }
    }
}