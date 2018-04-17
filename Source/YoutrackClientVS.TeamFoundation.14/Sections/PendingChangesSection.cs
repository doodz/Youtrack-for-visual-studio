using log4net;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer.Framework;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.TeamFoundation.TeamFoundation;

namespace YouTrackClientVS.TeamFoundation.Sections
{
    [TeamExplorerSection(Id, TeamExplorerPageIds.PendingChanges, 20)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PendingChangesSection : TeamExplorerBaseSection
    {
        private const string Id = "D4ED9EC5-0448-4E32-A68C-0CEC1941CB53";
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAppServiceProvider _appServiceProvider;
        private ITeamExplorerSection _section;

        [ImportingConstructor]
        public PendingChangesSection(
            IAppServiceProvider appServiceProvider,
            IYouTrackClientService youTrackClient,
            IPendingChangesSectionView sectionView) : base(sectionView)
        {
            _appServiceProvider = appServiceProvider;
            Title = "YouTrack";
        }

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            _appServiceProvider.YouTrackServiceProvider = ServiceProvider = e.ServiceProvider;
            _section = GetSection(Guid.Parse(TeamExplorerPageIds.PendingChanges));
            base.Initialize(sender, e);
        }

        public override void Loaded(object sender, SectionLoadedEventArgs e)
        {
            base.Loaded(sender, e);

            var page = (TeamExplorerPageBase)ServiceProvider.GetService(typeof(ITeamExplorerPage));

            var obj = (Microsoft.VisualStudio.TeamFoundation.VersionControl.PendingChanges.PendingChangesModelVS)page.Model;
            //var obj2 = (Microsoft.TeamFoundation.VersionControl.Client.IPendingCheckinPendingChanges)page.Model;

            var model = GetService<TeamExplorerModel>();

            var type = model.GetType();
            //var viewModel = GetService<TeamExplorerViewModel>();

            //var pages = new List<ITeamExplorerPage>();

            //if (viewModel.CurrentPage != null)
            //    pages.Add(viewModel.CurrentPage);
            //pages.AddRange(viewModel.UndockedPages);
            //var changesGuid = Guid.Parse(TeamExplorerPageIds.PendingChanges);
            //var changesPage = pages.FirstOrDefault(p => p.GetId() == changesGuid);

            //var view = changesPage.PageContent as UserControl;
            //(Microsoft.VisualStudio.TeamFoundation.VersionControl.PendingChanges.PendingChangesPageVS) model;
            //Microsoft.VisualStudio.TeamFoundation.VersionControl.
            // Microsoft.VisualStudio.TeamFoundation.
            //Microsoft.VisualStudio.TeamFoundation.VersionControl.PendingChanges.PendingChangesPageVS;

            //var _labeledTextBox = view.FindName("commentTextBox") as LabeledTextBox;

        }

        protected ITeamExplorerSection GetSection(Guid section)
        {
            return ((ITeamExplorerPage)ServiceProvider.GetService(typeof(ITeamExplorerPage))).GetSection(section);
        }
    }
}