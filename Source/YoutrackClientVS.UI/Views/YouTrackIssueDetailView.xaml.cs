using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;

namespace YouTrackClientVS.UI.Views
{
    [Export(typeof(IYouTrackIssueDetailView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class YouTrackIssueDetailView : UserControl, IYouTrackIssueDetailView
    {

        [ImportingConstructor]
        public YouTrackIssueDetailView(IYouTrackIssueDetailViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            var heightDescriptor = DependencyPropertyDescriptor.FromProperty(RowDefinition.HeightProperty, typeof(RowDefinition));
            heightDescriptor.AddValueChanged(FirstRow, HeightChanged);
        }

        private void HeightChanged(object sender, EventArgs e)
        {
            FirstRow.MaxHeight = Double.PositiveInfinity;
        }

        private void GoToCommit(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }


}
