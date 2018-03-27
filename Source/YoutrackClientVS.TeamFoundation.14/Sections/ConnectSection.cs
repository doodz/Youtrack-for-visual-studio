using log4net;
using Microsoft.TeamFoundation.Controls;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.TeamFoundation.TeamFoundation;

namespace YouTrackClientVS.TeamFoundation.Sections
{
    [TeamExplorerSection(Id, TeamExplorerPageIds.Connect, 20)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ConnectSection : TeamExplorerBaseSection
    {
        private readonly IAppServiceProvider _appServiceProvider;
        private ITeamExplorerSection _section;
        private const string Id = "BD7DD0FC-7B60-420E-8BF8-A4BEABC18A02";
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [ImportingConstructor]
        public ConnectSection(
            IAppServiceProvider appServiceProvider,
            IYouTrackClientService youTrackClient,
            IConnectSectionView sectionView) : base(sectionView)
        {
            _appServiceProvider = appServiceProvider;
            Title = youTrackClient.Title;
        }


        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            _appServiceProvider.YouTrackServiceProvider = ServiceProvider = e.ServiceProvider;
            _section = GetSection(TeamExplorerConnectionsSectionId);
            base.Initialize(sender, e);
        }




        protected ITeamExplorerSection GetSection(Guid section)
        {
            return ((ITeamExplorerPage)ServiceProvider.GetService(typeof(ITeamExplorerPage))).GetSection(section);
        }
    }
}