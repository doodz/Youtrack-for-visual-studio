using System.Collections.Generic;
using System.Threading.Tasks;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Clients.Enterprise
{
    public class EnterpriseTeamsClient : ApiClient, ITeamsClient
    {
        public EnterpriseTeamsClient(IEnterpriseYouTrackRestClient restClient, Connection connection) : base(restClient, connection)
        {

        }

        public Task<IEnumerable<Team>> GetTeams() // not needed
        {
            IEnumerable<Team> coll = new List<Team>();
            return Task.FromResult(coll);
        }
    }
}