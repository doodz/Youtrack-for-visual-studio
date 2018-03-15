using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IStorageService
    {
        Result SaveUserData(ConnectionData connectionData);
        Result<ConnectionData> LoadUserData();
        Result SaveYouTrackClientHistory(YouTrackClientHistory youTrackClientHistory);
        Result<YouTrackClientHistory> LoadYouTrackClientHistory();
    }
}