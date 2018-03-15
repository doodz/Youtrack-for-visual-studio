using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Contracts.Interfaces.ViewModels
{
    public interface IYouTrackAttachmentsViewModel : IViewModelWithErrorMessage
    {
        ReactiveCommand DownloadAttachmentCommand { get; }
        List<YouTrackAttachment> Attachments { get; }
        Task UpdateAttachments(string issueId);
        int AttachmentsCount { get; }
    }

    public interface IYouTrackCommentsViewModel : IViewModelWithErrorMessage
    {
        ReactiveCommand ReplyCommentCommand { get; }
        ReactiveCommand EditCommentCommand { get; }
        ReactiveCommand DeleteCommentCommand { get; }
        ReactiveCommand AddCommentCommand { get; }
        ReactiveCommand AddFileCommand { get; }
        List<YouTrackComment> Comments { get; }
        Task UpdateComments(string issueId);
        string CommentText { get; set; }
        int CommentsCount { get; }
    }
}