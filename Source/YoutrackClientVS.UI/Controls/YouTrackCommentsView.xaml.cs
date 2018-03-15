using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace YouTrackClientVS.UI.Controls
{
    /// <summary>
    /// Logique d'interaction pour YouTrackCommentsView.xaml
    /// </summary>
    public partial class YouTrackCommentsView : UserControl
    {
        public YouTrackCommentsView()
        {
            InitializeComponent();
        }

        private void GoToIssue(object sender, RequestNavigateEventArgs e)
        {
            //Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void GoToCommit(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
