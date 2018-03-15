using System.ComponentModel.Composition;
using System.Windows.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for PublishSectionView.xaml
    /// </summary>
    [Export(typeof(IPublishSectionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PublishSectionView : UserControl, IPublishSectionView
    {
        [ImportingConstructor]
        public PublishSectionView(IPublishSectionViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            vm.Initialize();
        }
    }
}
