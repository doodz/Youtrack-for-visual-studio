using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Clients
{
    public abstract class ApiClient
    {
        protected ApiClient(IYouTrackRestClient restClient, Connection connection)
        {
            RestClient = restClient;
            Connection = connection;
        }

        protected Connection Connection { get; private set; }

        protected IYouTrackRestClient RestClient { get; private set; }

    }
}