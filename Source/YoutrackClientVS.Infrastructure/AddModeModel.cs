using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Infrastructure
{
    public class AddModeModel
    {
        public GitCommentInline Inline { get; }

        public AddModeModel(GitCommentInline inline)
        {
            Inline = inline;
        }
    }
}