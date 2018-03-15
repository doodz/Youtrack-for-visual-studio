using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Events
{
    public class ClientHistoryChangedEvent
    {
        public YouTrackClientHistory Data { get; }

        public ClientHistoryChangedEvent(YouTrackClientHistory data)
        {
            Data = data;
        }
    }
}