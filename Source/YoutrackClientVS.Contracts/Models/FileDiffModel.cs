using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models.Tree;

namespace YouTrackClientVS.Contracts.Models
{
    public class FileDiffModel
    {
        public ITreeFile TreeFile { get; set; }
        public string FromCommit { get; set; }
        public string ToCommit { get; set; }
        public ICommentViewModel CommentViewModel { get; set; }
    }
}
