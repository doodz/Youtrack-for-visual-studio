using AutoMapper;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class TeamTypeConverter : ITypeConverter<Team, GitTeam>
    {
        public GitTeam Convert(Team source, GitTeam destination, ResolutionContext context)
        {
            return new GitTeam()
            {
                Name = source.Username
            };
        }
    }

    public class ReverseTeamTypeConverter : ITypeConverter<GitTeam, Team>
    {
        public Team Convert(GitTeam source, Team destination, ResolutionContext context)
        {
            return new Team()
            {
                Username = source.Name
            };
        }
    }
}