
using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface ICommandsService
    {
        void ShowDiffWindow(FileDiffModel parameter, int id);
        void Initialize(object package);
        void ShowYouTrackIssuesWindow();

        object ShowSideBySideDiffWindow(
            string content1,
            string content2,
            string fileDisplayName1,
            string fileDisplayName2,
            string caption,
            string tooltip,
            object vsFrame
            );
    }
}