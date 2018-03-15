using AutoMapper;
using YouTrack.REST.API.Mappings;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public static class BitBucketEnterpriseMappings
    {
        public static void AddEnterpriseProfile(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<EnterpriseToStandardMappingsProfile>();
        }
    }
}
