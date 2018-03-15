using GitClientVS.Contracts.Interfaces.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace GitClientVS.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour YouTrackIssuesWindowsContainer.xaml
    /// </summary>
    [Export(typeof(IYouTrackIssuesWindowsContainer))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class YouTrackIssuesWindowsContainer : UserControl, IYouTrackIssuesWindowsContainer
    {
        [ImportingConstructor]
        public YouTrackIssuesWindowsContainer()
        {
            InitializeComponent();
        }
    }
}
