using AutoMapper;
using System.Collections.Generic;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class IssueTypeConverter : ITypeConverter<Issue, YouTrackIssue>
    {
        public YouTrackIssue Convert(Issue source, YouTrackIssue destination, ResolutionContext context)
        {
            var result = new YouTrackIssue
            {

                Id = source.Id,
                JiraId = source.JiraId,
                Comments = source.Comments.MapTo<List<YouTrackComment>>(),
                Description = source.Description,
                EntityId = source.EntityId,
                Summary = source.Summary,
                Tags = source.Tags.MapTo<List<YouTrackSubValue<string>>>(),
                ReporterName = source.ReporterName,
                UpdaterName = source.UpdaterName,
                Updated = source.Updated,
                Created = source.Created,
                ProjectShortName = source.ProjectShortName,
                CommentsCount = source.CommentsCount
            };

            return result;
        }
    }
}