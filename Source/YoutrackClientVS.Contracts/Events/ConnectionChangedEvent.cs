using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Events
{
    public class ConnectionChangedEvent
    {
        public ConnectionData Data { get; }

        public ConnectionChangedEvent(ConnectionData data)
        {
            Data = data;
        }
    }
}
