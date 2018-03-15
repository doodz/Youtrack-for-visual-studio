using System.Collections.Generic;
using System.Threading.Tasks;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Interfaces
{
    public interface ITeamsClient
    {
        Task<IEnumerable<Team>> GetTeams();
    }
}