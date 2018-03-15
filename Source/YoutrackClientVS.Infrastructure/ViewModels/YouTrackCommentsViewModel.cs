using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IYouTrackCommentsViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class YouTrackCommentsViewModel : ViewModelBase, IYouTrackCommentsViewModel
    {
        private int _commentsCount;
        private string _commentText;
        public string IssueId { get; private set; }
        private readonly IUserInformationService _userInformationService;
        private readonly IYouTrackClientService _youTrackClientService;
        public ReactiveCommand ReplyCommentCommand { get; private set; }
        public ReactiveCommand EditCommentCommand { get; private set; }
        public ReactiveCommand DeleteCommentCommand { get; private set; }
        public ReactiveCommand AddCommentCommand { get; private set; }
        public ReactiveCommand AddFileCommand { get; private set; }

        private List<YouTrackComment> _comments;
        public List<YouTrackComment> Comments
        {
            get => _comments;
            private set => this.RaiseAndSetIfChanged(ref _comments, value);
        }

        public string CommentText
        {
            get => _commentText;
            set => this.RaiseAndSetIfChanged(ref _commentText, value);
        }

        public int CommentsCount
        {
            get => _commentsCount;
            private set => this.RaiseAndSetIfChanged(ref _commentsCount, value);
        }

        [ImportingConstructor]
        public YouTrackCommentsViewModel(IYouTrackClientService youTrackClientService, IUserInformationService userInformationService)
        {
            _youTrackClientService = youTrackClientService;
            _userInformationService = userInformationService;
        }


        public async Task UpdateComments(string issueId)
        {
            IssueId = issueId;
            Comments = (await _youTrackClientService.GetComments(issueId)).ToList();
            CommentsCount = Comments.Count;
        }




        private async Task ReplyToComment(YouTrackComment comment)
        {
            //TODO THE
            //if (string.IsNullOrEmpty(commentTree.ReplyContent))
            //    return;

            //var comment = commentTree.Comment;

            //var newComment = new GitComment()
            //{
            //    Content = new GitCommentContent() { Html = commentTree.ReplyContent },
            //    Parent = new GitCommentParent() { Id = comment.Id },
            //    Inline = comment.Inline != null ? new GitCommentInline() { Path = comment.Inline.Path } : null
            //};

            //var newServerComment = await _youTrackClientService.AddPullRequestComment(PullRequestId, newComment);
            //newServerComment.Inline = comment.Inline;
            //newServerComment.Parent = new GitCommentParent() { Id = comment.Id };

            //Comments.Add(newServerComment);

            //RebuildTree();
            //LastEditedComment = newServerComment;
        }

        private async Task EditComment(YouTrackComment comment)
        {
            if (string.IsNullOrEmpty(comment.Text))
                return;
            //TODO THE
            //commentTree.Comment.Content = new GitCommentContent() { Html = commentTree.EditContent };

            //var newServerComment = await _youTrackClientService.EditPullRequestComment(PullRequestId, commentTree.Comment);
            //newServerComment.Inline = commentTree.Comment.Inline;
            //newServerComment.Parent = commentTree.Comment.Parent;

            //var index = Comments.FindIndex(x => x.Id == newServerComment.Id);
            //Comments[index] = newServerComment;

            //RebuildTree();
            //LastEditedComment = newServerComment;
        }

        private async Task AddComment(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            //var comment = new GitComment()
            //{
            //    Content = new GitCommentContent() { Html = text },
            //    Inline = inline != null ? new GitCommentInline()
            //    {
            //        Path = inline.Path,
            //        From = inline.From,
            //        To = inline.To
            //    } : null
            //};

            //var newServerComment = await _youTrackClientService.AddPullRequestComment(PullRequestId, comment);
            //newServerComment.Inline = comment.Inline;
            //Comments.Add(newServerComment);

            //RebuildTree();
            //LastEditedComment = newServerComment;
        }

        private async Task DeleteComment(YouTrackComment comment)
        {

            //await _youTrackClientService.DeletePullRequestComment(PullRequestId, comment.Id, comment.Version);

            //var index = Comments.FindIndex(x => x.Id == comment.Id);
            //Comments[index].IsDeleted = true;

            //RebuildTree();
            //LastEditedComment = Comments[index];
        }

        public void InitializeCommands()
        {
            ReplyCommentCommand = ReactiveCommand.CreateFromTask<YouTrackComment>(ReplyToComment);
            EditCommentCommand = ReactiveCommand.CreateFromTask<YouTrackComment>(EditComment);
            DeleteCommentCommand = ReactiveCommand.CreateFromTask<YouTrackComment>(DeleteComment);
            AddCommentCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    await AddComment(CommentText);
                    CommentText = string.Empty;
                },
                Changed.Select(y => !string.IsNullOrEmpty(CommentText)));

            //AddFileCommand = ReactiveCommand.CreateFromTask<YouTrackComment>(async inline =>
            //{
            //    await AddComment(inline, FileLevelCommentText);
            //    FileLevelCommentText = string.Empty;
            //}, Changed.Select(y => !string.IsNullOrEmpty(FileLevelCommentText)));

            //AddInlineCommentCommand = ReactiveCommand.CreateFromTask<GitCommentInline>(async inline =>
            //{
            //    await AddComment(inline, InlineCommentText);
            //    InlineCommentText = string.Empty;
            //}, Changed.Select(y => !string.IsNullOrEmpty(InlineCommentText)));
        }


        public string ErrorMessage { get; set; }
        public IEnumerable<ReactiveCommand> ThrowableCommands => new[] { ReplyCommentCommand, EditCommentCommand, DeleteCommentCommand, AddCommentCommand };
    }
}