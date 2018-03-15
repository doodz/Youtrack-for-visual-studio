using ReactiveUI;
using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Contracts.Models.Tree
{
    public interface ICommentTree
    {
        ReactiveList<ICommentTree> Comments { get; set; }
        GitComment Comment { get; set; }
        bool IsExpanded { get; set; }
        bool AllDeleted { get; }
        void DeleteCurrentComment();
        void AddComment(GitComment comment);
        bool IsEditExpanded { get; set; }
        bool IsReplyExpanded { get; set; }
        string EditContent { get; set; }
        string ReplyContent { get; set; }
    }
}