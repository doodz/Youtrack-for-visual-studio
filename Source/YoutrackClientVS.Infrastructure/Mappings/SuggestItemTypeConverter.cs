using AutoMapper;
using YouTrack.REST.API.Mappings;
using YouTrack.REST.API.Models.Standard;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class SuggestItemTypeConverter : ITypeConverter<SuggestItem, YouTrackSuggestItem>
    {
        public YouTrackSuggestItem Convert(SuggestItem source, YouTrackSuggestItem destination, ResolutionContext context)
        {
            var result = new YouTrackSuggestItem
            {
                Caret = source.Caret,
                Complete = source.Complete,
                Completion = source.Completion.MapTo<YouTrackCompletion>(),
                Match = source.Match.MapTo<YouTrackCompletion>(),
                Description = source.Description,
                HtmlDescription = source.HtmlDescription,
                Option = source.Option,
                Prefix = source.Prefix,
                StyleClass = source.StyleClass.MapTo<YouTrackStyleClass>(),
                Suffix = source.Suffix
            };


            return result;
        }

    }
}