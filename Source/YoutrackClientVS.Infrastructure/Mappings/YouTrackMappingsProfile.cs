using AutoMapper;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.Utils;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class YouTrackMappingsProfile : Profile
    {
        public YouTrackMappingsProfile()
        {
            CreateMap<Repository, GitRemoteRepository>().ConvertUsing<RepositoryTypeConverter>();
            CreateMap<GitRemoteRepository, Repository>().ConvertUsing<ReverseRepositoryTypeConverter>();

            CreateMap<GitPullRequest, PullRequest>().ConvertUsing<ReversePullRequestTypeConverter>();
            CreateMap<PullRequest, GitPullRequest>().ConvertUsing<PullRequestTypeConverter>();

            CreateMap<PullRequestOptions, GitPullRequestStatus>().ConvertUsing<PullRequestOptionsTypeConverter>();
            CreateMap<GitPullRequestStatus, PullRequestOptions>().ConvertUsing<ReversePullRequestOptionsTypeConverter>();

            CreateMap<Team, GitTeam>().ConvertUsing<TeamTypeConverter>();
            CreateMap<GitTeam, Team>().ConvertUsing<ReverseTeamTypeConverter>();

            CreateMap<Commit, GitCommit>()
                .ForMember(dto => dto.Date, e => e.MapFrom(o => TimeConverter.GetDate(o.Date)))
                .ForMember(dto => dto.Author, e => e.MapFrom(o => o.Author.User));

            CreateMap<GitCommit, Commit>();

            CreateMap(typeof(IteratorBasedPage<>), typeof(PageIterator<>));

            CreateMap<Parent, GitCommentParent>();



            CreateMap<Content, GitCommentContent>();
            CreateMap<Branch, GitBranch>();

            CreateMap<GitBranch, Branch>();


            CreateMap<GitUser, UserShort>();
            CreateMap<GitUser, GitUser>();

            CreateMap<User, GitUser>();
            CreateMap<User, UserShort>();
            CreateMap<UserShort, GitUser>();
            CreateMap<Links, GitLinks>();
            CreateMap<Link, GitLink>();
            CreateMap<GitMergeRequest, MergeRequest>();

            CreateMap<Issue, YouTrackIssue>().ConvertUsing<IssueTypeConverter>(); ;
            CreateMap<Comment, YouTrackComment>().ConvertUsing<CommentTypeConverter>();
            CreateMap<User, YouTrackUser>();
            CreateMap<Attachment, YouTrackAttachment>();
        }
    }
}