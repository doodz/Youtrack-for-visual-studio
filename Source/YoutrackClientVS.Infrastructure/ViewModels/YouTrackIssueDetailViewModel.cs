using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.Tree;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IYouTrackIssueDetailViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class YouTrackIssueDetailViewModel : ViewModelBase, IYouTrackIssueDetailViewModel
    {
        private readonly IYouTrackClientService _youTrackClientService;
        private readonly IUserInformationService _userInformationService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IVsTools _vsTools;
        private string _errorMessage;
        private bool _isLoading;
        private ReactiveCommand _initializeCommand;
        private ReactiveCommand<Unit, Unit> _approveCommand;
        private ReactiveCommand<Unit, Unit> _disapproveCommand;
        private ReactiveCommand<Unit, Unit> _declineCommand;
        private ReactiveCommand<Unit, Unit> _mergeCommand;
        private ReactiveCommand<Unit, Unit> _confirmationMergeCommand;
        private ReactiveCommand<Unit, Unit> _confirmationDeclineCommand;
        private ReactiveCommand<Unit, Unit> _refreshIssuCommand;

        private YouTrackIssue _issue;
        private string _mainSectionCommandText;
        private Theme _currentTheme;
        private Uri _issueLink;
        private ReactiveList<PullRequestActionModel> _actionCommands;
        private bool _hasAuthorApproved;
        private readonly IEventAggregatorService _eventAggregatorService;
        private readonly IDataNotifier _dataNotifier;

        public string PageTitle => "YouTrack issue Details";

        public string MainSectionCommandText
        {
            get => _mainSectionCommandText;
            set => this.RaiseAndSetIfChanged(ref _mainSectionCommandText, value);
        }

        public YouTrackIssue Issue
        {
            get => _issue;
            set => this.RaiseAndSetIfChanged(ref _issue, value);
        }


        public Uri IssueLink
        {
            get => _issueLink;
            set => this.RaiseAndSetIfChanged(ref _issueLink, value);
        }


        public Theme CurrentTheme
        {
            get => _currentTheme;
            set => this.RaiseAndSetIfChanged(ref _currentTheme, value);
        }

        public ReactiveList<PullRequestActionModel> ActionCommands
        {
            get => _actionCommands;
            set => this.RaiseAndSetIfChanged(ref _actionCommands, value);
        }

        public bool HasAuthorApproved
        {
            get => _hasAuthorApproved;
            set => this.RaiseAndSetIfChanged(ref _hasAuthorApproved, value);
        }

        public IPullRequestDiffViewModel PullRequestDiffViewModel { get; set; }
        public IYouTrackCommentsViewModel YouTrackCommentsViewModel { get; set; }
        public IYouTrackAttachmentsViewModel YouTrackAttachmentsViewModel { get; set; }

        public IEnumerable<ReactiveCommand> ThrowableCommands =>
            new[] { _initializeCommand, _mergeCommand, _approveCommand, _disapproveCommand, _declineCommand }
                .Concat(YouTrackCommentsViewModel.ThrowableCommands).Concat(
                    PullRequestDiffViewModel.ThrowableCommands).Concat(YouTrackAttachmentsViewModel.ThrowableCommands);

        public IEnumerable<ReactiveCommand> LoadingCommands => new[]
            {_initializeCommand, _approveCommand, _disapproveCommand, _declineCommand, _mergeCommand};

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        [ImportingConstructor]
        public YouTrackIssueDetailViewModel(
            IYouTrackClientService youTrackClientService,
            IGitService gitService,
            ICommandsService commandsService,
            IUserInformationService userInformationService,
            IEventAggregatorService eventAggregatorService,
            IMessageBoxService messageBoxService,
            IDataNotifier dataNotifier,
            IPullRequestDiffViewModel pullRequestDiffViewModel,
            IYouTrackCommentsViewModel youTrackCommentsViewModel,
            IYouTrackAttachmentsViewModel youTrackAttachmentsViewModel
        )
        {
            _youTrackClientService = youTrackClientService;
            _userInformationService = userInformationService;
            _messageBoxService = messageBoxService;
            _eventAggregatorService = eventAggregatorService;
            _dataNotifier = dataNotifier;

            CurrentTheme = userInformationService.CurrentTheme;
            PullRequestDiffViewModel = pullRequestDiffViewModel;
            YouTrackCommentsViewModel = youTrackCommentsViewModel;
            YouTrackAttachmentsViewModel = youTrackAttachmentsViewModel;
        }

        protected override IEnumerable<IDisposable> SetupObservables()
        {
            yield return _eventAggregatorService.GetEvent<ThemeChangedEvent>()
                .Subscribe(ev => { CurrentTheme = ev.Theme; });

            this.WhenAnyObservable(
                    x => x._approveCommand,
                    x => x._declineCommand,
                    x => x._disapproveCommand,
                    x => x._mergeCommand,
                    x => x._refreshIssuCommand
                )
                .Where(x => Issue != null)
                .Select(x => Issue.Id)
                .InvokeCommand(_initializeCommand);

            this.WhenAnyValue(x => x.Issue).Where(x => x != null).Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(PageTitle));
            });
        }

        public ICommand InitializeCommand => _initializeCommand;
        public ICommand RefreshIssuCommand => _refreshIssuCommand;

        public void InitializeCommands()
        {
            _initializeCommand = ReactiveCommand.CreateFromTask<string>(LoadIssueDatas);
            _refreshIssuCommand = ReactiveCommand.Create(() => { });
            _approveCommand = ReactiveCommand.CreateFromTask(async _ =>
            {
                //await _youTrackClientService.ApprovePullRequest(Issue.Id);
                _dataNotifier.ShouldUpdate = true;
            });

            _disapproveCommand = ReactiveCommand.CreateFromTask(async _ =>
            {
                //await _youTrackClientService.DisapprovePullRequest(Issue.Id);
                _dataNotifier.ShouldUpdate = true;
            });

            _declineCommand = ReactiveCommand.CreateFromTask(async _ =>
            {
                //await _youTrackClientService.DeclinePullRequest(Issue.Id, Issue.Version);
                _dataNotifier.ShouldUpdate = true;
            });

            _mergeCommand = ReactiveCommand.CreateFromTask(async _ =>
            {
                await MergePullRequest();
                _dataNotifier.ShouldUpdate = true;
            });
            _confirmationMergeCommand = ReactiveCommand.CreateFromTask(_ => RunMergeConfirmation());
            _confirmationDeclineCommand = ReactiveCommand.CreateFromTask(_ => RunDeclineConfirmation());
        }

        private Task RunDeclineConfirmation()
        {
            _messageBoxService.ExecuteCommandWithConfirmation(
                "Declining Pull Request",
                "Do you really want to decline this pull request?",
                _declineCommand
            );

            return Task.CompletedTask;
        }

        private Task RunMergeConfirmation()
        {
            _messageBoxService.ExecuteCommandWithConfirmation(
                "Merging Pull Request",
                "Do you really want to merge this pull request?",
                _mergeCommand
            );

            return Task.CompletedTask;
        }


        private async Task LoadIssueDatas(string id)
        {
            var tasks = new[]
            {
                GetIssueInfo(id),
                GetComments(id),
                GetAttachments(id)
            };

            await Task.WhenAll(tasks);
            //TODO THE
            //PullRequestDiffViewModel.FromCommit = Issue.SourceBranch.Target.Hash;
            //PullRequestDiffViewModel.ToCommit = Issue.DestinationBranch.Target.Hash;

            //UpdateCommentsCountInFiles(PullRequestDiffViewModel.FilesTree);
        }

        private void UpdateCommentsCountInFiles(IEnumerable<ITreeFile> files)
        {
            foreach (var treeFile in files)
            {
                treeFile.Comments = PullRequestDiffViewModel.CommentViewModel.Comments.Where(x => !x.IsDeleted)
                    .Count(x => x.Inline != null && treeFile.FileDiff != null &&
                                x.Inline.Path == treeFile.FileDiff.DisplayFileName);
                UpdateCommentsCountInFiles(treeFile.Files);
            }
        }

        private async Task GetIssueInfo(string id)
        {
            var issue = await _youTrackClientService.GetIssue(id);
            CreatePullRequestCommands(null);
            Issue = issue;
            IssueLink = _youTrackClientService.GetIssueUri(id);
        }

        private async Task GetComments(string id)
        {
            //var comments = await _youTrackClientService.GetComments(id);

            await YouTrackCommentsViewModel.UpdateComments(id);
            //CreatePullRequestCommands(null);
        }

        private async Task GetAttachments(string id)
        {
            await YouTrackAttachmentsViewModel.UpdateAttachments(id);
        }

        private async Task CreateComments(long id)
        {
            await PullRequestDiffViewModel.UpdateComments(id);
        }

        private async Task CreateCommits(long id)
        {
            var commits = (await _youTrackClientService.GetPullRequestCommits(id)).ToList();
            PullRequestDiffViewModel.AddCommits(commits);
        }

        private async Task CreateDiffContent(long id)
        {
            var fileDiffs = (await _youTrackClientService.GetPullRequestDiff(id)).ToList();

            PullRequestDiffViewModel.AddFileDiffs(fileDiffs);
        }

        private void CreatePullRequestCommands(GitPullRequest pullRequest)
        {
            //TODO THE
            //var isApproved = true;
            //var isApproveAvailable = false;

            //foreach (var reviewer in pullRequest.Reviewers)
            //    if (reviewer.Key.Username == _userInformationService.ConnectionData.UserName)
            //    {
            //        isApproveAvailable = true;
            //        isApproved = reviewer.Value;
            //    }

            //var approvesCount = pullRequest.Reviewers.Count(x => x.Value);
            //var author = pullRequest.Reviewers.FirstOrDefault(x => x.Key.Username == pullRequest.Author.Username);
            //HasAuthorApproved = author.Value;
            //if (author.Key != null)
            //    pullRequest.Reviewers.Remove(author.Key);

            //ActionCommands = new ReactiveList<PullRequestActionModel>
            //{
            //    new PullRequestActionModel("Merge", _confirmationMergeCommand),
            //    new PullRequestActionModel("Decline", _confirmationDeclineCommand),
            //    !isApproveAvailable || !isApproved
            //        ? new PullRequestActionModel($"Approve ({approvesCount})", _approveCommand)
            //        : new PullRequestActionModel($"Unapprove ({approvesCount})", _disapproveCommand)
            //};
        }


        private async Task MergePullRequest()
        {
            //TODO THE
            //var gitMergeRequest = new GitMergeRequest()
            //{
            //    CloseSourceBranch = Issue.CloseSourceBranch,
            //    Id = Issue.Id,
            //    MergeStrategy = "merge_commit",
            //    Version = Issue.Version
            //};

            //await _youTrackClientService.MergePullRequest(gitMergeRequest);
        }
    }
}