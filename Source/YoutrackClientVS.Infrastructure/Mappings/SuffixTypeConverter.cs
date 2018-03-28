using AutoMapper;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class SuffixTypeConverter
        : ITypeConverter<Suffix, YouTrackSuffix>
    {
        public YouTrackSuffix Convert(Suffix source, YouTrackSuffix destination, ResolutionContext context)
        {
            if (source == Suffix.Empty)
            {
                return YouTrackSuffix.Empty;
            }

            return YouTrackSuffix.Empty;
        }
    }
}