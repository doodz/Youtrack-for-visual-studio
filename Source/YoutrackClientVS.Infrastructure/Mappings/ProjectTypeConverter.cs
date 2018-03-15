using AutoMapper;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class ProjectTypeConverter : ITypeConverter<Project, YouTrackProject>
    {
        public YouTrackProject Convert(Project source, YouTrackProject destination, ResolutionContext context)
        {
            return new YouTrackProject()
            {
                Name = source.Name,
                Description = source.Description,
                ShortName = source.ShortName,
                Versions = source.Versions

            };
        }
    }

    public class ReverseProjectTypeConverter : ITypeConverter<YouTrackProject, Project>
    {
        public Project Convert(YouTrackProject source, Project destination, ResolutionContext context)
        {
            return new Project()
            {
                Name = source.Name,
                Description = source.Description,
                ShortName = source.ShortName,
                Versions = source.Versions
            };
        }
    }
}