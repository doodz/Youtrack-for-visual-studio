using AutoMapper;
using YouTrack.REST.API.Mappings.Converters;
using YouTrack.REST.API.Models.Enterprise;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Mappings
{
    public class EnterpriseToStandardMappingsProfile : Profile
    {
        public EnterpriseToStandardMappingsProfile()
        {
            CreateMap<RepositoryV1, Repository>().ConvertUsing<RepositoryV1TypeConverter>(); //this is not rly enterprise related


            CreateMap<EnterpriseLink, Link>();
            CreateMap<Link, EnterpriseLink>();

            CreateMap<EnterpriseUser, User>();
            CreateMap<User, EnterpriseUser>();

            CreateMap<EnterpriseLinks, Links>().ConvertUsing<EnterpriseLinksTypeConverter>();
            CreateMap<Links, EnterpriseLinks>().ConvertUsing<EnterpriseLinksTypeConverter>();
            CreateMap<EnterpriseBranch, Branch>().ConvertUsing<EnterpriseBranchTypeConverter>();
            CreateMap<EnterpriseParticipant, Participant>().ConvertUsing<EnterpriseParticipantTypeConverter>();

            CreateMap<EnterprisePullRequest, PullRequest>().ConvertUsing<EnterprisePullRequestTypeConverter>();
            CreateMap<PullRequest, EnterprisePullRequest>().ConvertUsing<EnterprisePullRequestTypeConverter>();
            CreateMap<EnterpriseBranchSource, Source>().ConvertUsing<EnterpriseBranchSourceTypeConverter>();
            CreateMap<Source, EnterpriseBranchSource>().ConvertUsing<EnterpriseBranchSourceTypeConverter>();

            CreateMap<EnterprisePullRequestOptions, PullRequestOptions>();
            CreateMap<PullRequestOptions, EnterprisePullRequestOptions>();
            CreateMap<EnterpriseUser, UserShort>();
            CreateMap<EnterpriseCommit, Commit>().ConvertUsing<EnterpriseCommitTypeConverter>();


            CreateMap<EnterpriseRepository, Repository>().ConvertUsing<EnterpriseRepositoryTypeConverter>();
            CreateMap(typeof(IteratorBasedPage<>), typeof(IteratorBasedPage<>));
        }
    }
}