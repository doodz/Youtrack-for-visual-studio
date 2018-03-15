using AutoMapper;
using YouTrack.REST.API.Models.Enterprise;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Mappings.Converters
{
    public class EnterpriseBranchTypeConverter : ITypeConverter<EnterpriseBranch, Branch>
    {
        public Branch Convert(EnterpriseBranch source, Branch destination, ResolutionContext context)
        {
            return new Branch()
            {
                Name = source.DisplayId,
                IsDefault = source.IsDefault,
                Target = new Commit()
                {
                    Hash = source.LatestCommitId
                },
            };
        }
    }
}