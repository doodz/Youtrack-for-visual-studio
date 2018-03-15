using AutoMapper;
using System.Collections.Generic;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class CommentTypeConverter : ITypeConverter<Comment, YouTrackComment>
    {
        public YouTrackComment Convert(Comment source, YouTrackComment destination, ResolutionContext context)
        {
            var result = new YouTrackComment
            {

                Id = source.Id,
                Author = source.Author,
                AuthorFullName = source.AuthorFullName,
                IssueId = source.IssueId,
                ParentId = source.ParentId,
                IsDeleted = source.IsDeleted,
                JiraId = source.JiraId,
                Text = source.Text,
                ShownForIssueAuthor = source.ShownForIssueAuthor,
                Created = source.Created,
                Updated = source.Updated,
                PermittedGroup = source.PermittedGroup,
                Replies = source.Replies.MapTo<List<YouTrackComment>>()

            };


            return result;
        }
    }
}