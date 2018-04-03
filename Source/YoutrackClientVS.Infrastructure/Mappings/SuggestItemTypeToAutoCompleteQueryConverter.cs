using AutoMapper;
using System.Text;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.AutoCompleteTextBox;

namespace YouTrackClientVS.Infrastructure.Mappings
{
    public class SuggestItemTypeToAutoCompleteQueryConverter : ITypeConverter<YouTrackSuggestItem, AutoCompleteQueryResult>
    {
        public AutoCompleteQueryResult Convert(YouTrackSuggestItem source, AutoCompleteQueryResult destination, ResolutionContext context)
        {
            var result = new AutoCompleteQueryResult
            {
                Description = source.Description,
                Cursor = source.Completion.Start
            };


            var sb = new StringBuilder();
            sb.Append(source.Prefix);
            sb.Append(source.Option);
            sb.Append(source.Suffix);
            //sb.Append(" ");
            result.Title = sb.ToString().TrimEnd();

            return result;
        }

    }
}