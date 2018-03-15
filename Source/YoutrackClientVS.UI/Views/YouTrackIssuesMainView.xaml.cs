using System.ComponentModel.Composition;
using System.Windows.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for YouTrackIssuesMainView.xaml
    /// </summary>
    [Export(typeof(IYouTrackIssuesMainView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class YouTrackIssuesMainView : UserControl, IYouTrackIssuesMainView
    {
        private readonly IYouTrackIssuesMainViewModel _vm;

        [ImportingConstructor]
        public YouTrackIssuesMainView(IYouTrackIssuesMainViewModel vm)
        {
            _vm = vm;
            DataContext = vm;
            InitializeComponent();
        }

        private void PullRequestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PullRequestListBox.UnselectAll();
        }
    }
}
