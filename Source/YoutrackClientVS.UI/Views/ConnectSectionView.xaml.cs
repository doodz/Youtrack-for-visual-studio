using System.ComponentModel.Composition;
using System.Windows.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for ConnectSectionView.xaml
    /// </summary>
    [Export(typeof(IConnectSectionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ConnectSectionView : UserControl, IConnectSectionView
    {

        [ImportingConstructor]
        public ConnectSectionView(IConnectSectionViewModel connectSectionViewModel)
        {
            InitializeComponent();
            DataContext = connectSectionViewModel;
            connectSectionViewModel.Initialize();
        }
    }
}
