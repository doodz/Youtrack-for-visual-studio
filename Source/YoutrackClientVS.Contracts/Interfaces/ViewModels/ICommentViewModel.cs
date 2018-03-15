using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.Tree;

namespace YouTrackClientVS.Contracts.Interfaces.ViewModels
{
    public interface ICommentViewModel : IViewModelWithErrorMessage
    {
        ReactiveCommand ReplyCommentCommand { get; }
        ReactiveCommand EditCommentCommand { get; }
        ReactiveCommand DeleteCommentCommand { get; }
        ReactiveCommand AddCommentCommand { get; }
        ReactiveCommand AddFileLevelCommentCommand { get; }
        ReactiveCommand AddInlineCommentCommand { get; }
        string CurrentUserName { get; }

        string CommentText { get; set; }
        string FileLevelCommentText { get; set; }
        string InlineCommentText { get; set; }

        List<ICommentTree> CommentTree { get; }
        int CommentsCount { get; }
        long PullRequestId { get; }
        List<ICommentTree> InlineCommentTree { get; }
        GitComment LastEditedComment { get; }

        Task UpdateComments(long pullRequestId);
        List<GitComment> Comments { get; set; }
    }
}