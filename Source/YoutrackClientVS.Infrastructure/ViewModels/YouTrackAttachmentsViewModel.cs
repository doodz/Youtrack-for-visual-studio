using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Infrastructure.ViewModels
{
    [Export(typeof(IYouTrackAttachmentsViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class YouTrackAttachmentsViewModel : ViewModelBase, IYouTrackAttachmentsViewModel
    {
        public ReactiveCommand DownloadAttachmentCommand { get; private set; }
        private int _attachmentsCount;
        public string IssueId { get; private set; }
        private readonly IYouTrackClientService _youTrackClientService;
        private readonly IUserInformationService _userInformationService;
        private List<YouTrackAttachment> _attachments;
        public List<YouTrackAttachment> Attachments
        {
            get => _attachments;
            private set => this.RaiseAndSetIfChanged(ref _attachments, value);
        }
        public int AttachmentsCount
        {
            get => _attachmentsCount;
            private set => this.RaiseAndSetIfChanged(ref _attachmentsCount, value);
        }

        [ImportingConstructor]
        public YouTrackAttachmentsViewModel(IYouTrackClientService youTrackClientService, IUserInformationService userInformationService)
        {
            _youTrackClientService = youTrackClientService;
            _userInformationService = userInformationService;
        }


        public async Task UpdateAttachments(string issueId)
        {
            IssueId = issueId;
            Attachments = (await _youTrackClientService.GetAttachments(issueId)).ToList();
            AttachmentsCount = Attachments.Count;
        }

        public void InitializeCommands()
        {
            DownloadAttachmentCommand = ReactiveCommand.CreateFromTask<YouTrackAttachment>(DownloadAttachment);

        }

        private Task<bool> DownloadAttachment(YouTrackAttachment attachment)
        {
            throw new System.NotImplementedException();
        }


        public string ErrorMessage { get; set; }
        public IEnumerable<ReactiveCommand> ThrowableCommands => new[] { DownloadAttachmentCommand };
    }
}