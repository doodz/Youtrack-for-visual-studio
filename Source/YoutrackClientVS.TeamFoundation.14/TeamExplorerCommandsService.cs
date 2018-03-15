using System;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using YouTrackClientVS.Contracts.Interfaces.Services;

namespace YouTrackClientVS.TeamFoundation
{
    [Export(typeof(ITeamExplorerCommandsService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TeamExplorerCommandsService : ITeamExplorerCommandsService
    {
        private readonly ITeamExplorer _teamExplorer; // good morning MEF, I'm empty field but if I'm not here you won't load my plugin silently :)
        private readonly IAppServiceProvider _appServiceProvider;

        [ImportingConstructor]
        public TeamExplorerCommandsService(IAppServiceProvider appServiceProvider)
        {
            _appServiceProvider = appServiceProvider;
        }

        public void NavigateToHomePage()
        {
            _appServiceProvider.GetService<ITeamExplorer>()?.NavigateToPage(new Guid(TeamExplorerPageIds.Home), null);
        }
    }
}
