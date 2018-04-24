using System.ComponentModel.Composition;
using System.Windows.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for CreateIssueView.xaml
    /// </summary>
    [Export(typeof(ICreateIssueView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CreateIssueView : UserControl, ICreateIssueView
    {

        [ImportingConstructor]
        public CreateIssueView(ICreateIssueViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

    }
}
