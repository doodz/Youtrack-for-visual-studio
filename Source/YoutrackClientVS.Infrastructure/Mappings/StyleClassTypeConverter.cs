using AutoMapper;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class StyleClassTypeConverter
        : ITypeConverter<StyleClass, YouTrackStyleClass>
    {
        public YouTrackStyleClass Convert(StyleClass source, YouTrackStyleClass destination, ResolutionContext context)
        {
            if (source == StyleClass.Keyword)
            {
                return YouTrackStyleClass.Keyword;
            }

            return YouTrackStyleClass.Keyword;
        }
    }
}