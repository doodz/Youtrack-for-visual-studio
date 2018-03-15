using System.Collections.Generic;
using System.Threading.Tasks;
using YouTrack.REST.API.Helpers;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Clients.Standard
{
    public class TeamsClient : ApiClient, ITeamsClient
    {
        public TeamsClient(IYouTrackRestClient restClient, Connection connection) : base(restClient, connection)
        {

        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            var url = ApiUrls.Teams();
            return await RestClient.GetAllPages<Team>(url);
        }
    }
}