using log4net;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Microsoft.TeamFoundation.Controls.WPF;
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
            _section = GetSection(TeamExplorerConnectionsSectionId);
            base.Initialize(sender, e);
        }

        public override void Loaded(object sender, SectionLoadedEventArgs e)
        {
            base.Loaded(sender, e);

            var res = ((ITeamExplorerPage) ServiceProvider.GetService(typeof(ITeamExplorerPage)));
            //var service = GetService<TeamExplorerViewModel>();
            //var pages = new List<ITeamExplorerPage>();
            //if (service.CurrentPage != null)
            //    pages.Add(service.CurrentPage);
            //pages.AddRange(service.UndockedPages);
            //var changesGuid = Guid.Parse(TeamExplorerPageIds.PendingChanges);
            //var changesPage = pages.FirstOrDefault(p => p.GetId() == changesGuid);
            //var view = changesPage.PageContent as UserControl;

            //Microsoft.VisualStudio.TeamFoundation.VersionControl.PendingChanges.PendingChangesPageVS

            //var _labeledTextBox = view.FindName("commentTextBox") as LabeledTextBox;
        }

        protected ITeamExplorerSection GetSection(Guid section)
        {
            return ((ITeamExplorerPage)ServiceProvider.GetService(typeof(ITeamExplorerPage))).GetSection(section);
        }
    }
}