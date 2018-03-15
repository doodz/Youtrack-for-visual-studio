using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace YouTrackClientVS.UI.Controls
{
    /// <summary>
    /// Interaction logic for PullRequestDiffView.xaml
    /// </summary>
    public partial class PullRequestDiffView : UserControl
    {
       
        public PullRequestDiffView()
        {
            InitializeComponent();
        }


        private void GoToCommit(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

       
    }
}
